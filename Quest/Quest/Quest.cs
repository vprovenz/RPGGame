using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine;

namespace Quest
{
    public partial class Quest : Form
    {
        private Player player;
        private Gremlin currentMonster;

        public Quest()
        {
            InitializeComponent();

            player = new Player(10, 10, 20, 0, 1);
            MoveTo(Swamp.LocationByID(Swamp.LOCATION_ID_HOME));
            player.Inventory.Add(new Inventory(Swamp.ItemByID(Swamp.ITEM_ID_SLINGSHOT), 1));

            lblPoints.Text = player.CurrentPoints.ToString();
            lblGold.Text = player.Gold.ToString();
            lblExperience.Text = player.ExperiencePoints.ToString();
            lblLevel.Text = player.Level.ToString();
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(player.CurrentLocation.LocationToWest);
        }

        private void MoveTo(Location newLocation)
        {
            //See if a pass is needed for location
            if (!player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "You need a " + newLocation.ItemRequiredToEnter.Name + " to enter this location." + Environment.NewLine;
                return;
            }
            
            player.CurrentLocation = newLocation;

            //Show available movements
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            //Display location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            //Heal the player
            player.CurrentPoints = player.MaximumPoints;
            lblPoints.Text = player.CurrentPoints.ToString();

            //See if quest exists at this location
            if (newLocation.QuestAvailableHere != null)
            {
                handleQuest(newLocation);
            }

            // Does the location have a monster?
            if (newLocation.GremlinLivingHere != null)
            {
                rtbMessages.Text += "You see a " + newLocation.GremlinLivingHere.Name + Environment.NewLine;

                // Make a new monster, using the values from the standard monster in the Swamp.Ogre list
                Gremlin standardMonster = Swamp.GremlinByID(newLocation.GremlinLivingHere.ID);

                currentMonster = new Gremlin(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                    standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentPoints, standardMonster.MaximumPoints);

                foreach (Treasure lootItem in standardMonster.TreasureChest)
                {
                    currentMonster.TreasureChest.Add(lootItem);
                }

                comboBoxWeapons.Visible = true;
                comboBoxPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                currentMonster = null;

                comboBoxWeapons.Visible = false;
                comboBoxPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }

            // Refresh player's inventory list
            UpdateInventoryListInUI();

            // Refresh player's quest list
            UpdateQuestListInUI();

            // Refresh player's weapons combobox
            UpdateWeaponListInUI();

            // Refresh player's potions combobox
            UpdatePotionListInUI();
        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach (Inventory inventoryItem in player.Inventory)
            {
                if (inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Item.Name, inventoryItem.Quantity.ToString() });
                }
            }
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (AdventureQuest playerQuest in player.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Quest.Name, playerQuest.IsCompleted.ToString() });
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (Inventory inventoryItem in player.Inventory)
            {
                if (inventoryItem.Item is Weapon)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Item);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                //hide the weapon box and "Use" button
                comboBoxWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                comboBoxWeapons.DataSource = weapons;
                comboBoxWeapons.DisplayMember = "Name";
                comboBoxWeapons.ValueMember = "ID";

                comboBoxWeapons.SelectedIndex = 0;
            }
        }

        private void UpdatePotionListInUI()
        {
            List<Elixir> healingElixirs = new List<Elixir>();

            foreach (Inventory inventoryItem in player.Inventory)
            {
                if (inventoryItem.Item is Elixir)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        healingElixirs.Add((Elixir)inventoryItem.Item);
                    }
                }
            }

            if (healingElixirs.Count == 0)
            {
                //hide the elixir box and "Use" button
                comboBoxPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                comboBoxPotions.DataSource = healingElixirs;
                comboBoxPotions.DisplayMember = "Name";
                comboBoxPotions.ValueMember = "ID";

                comboBoxPotions.SelectedIndex = 0;
            }
        }

        private void handleQuest(Location newLocation)
        {
            //See if the player already has the quest, and if they've completed it
            bool playerAlreadyHasQuest = player.HasThisQuest(newLocation.QuestAvailableHere);
            bool playerAlreadyCompletedQuest = player.CompletedThisQuest(newLocation.QuestAvailableHere);

            if (playerAlreadyHasQuest)
            {
                if (!playerAlreadyCompletedQuest)
                {
                    bool playerHasAllItemsToCompleteQuest = player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                    if (playerHasAllItemsToCompleteQuest)
                    {
                        rtbMessages.Text += Environment.NewLine;
                        rtbMessages.Text += "You complete the '" + newLocation.QuestAvailableHere.Name + "' quest." + Environment.NewLine;

                        //Remove quest items from inventory
                        player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                        //Give rewards
                        rtbMessages.Text += "You receive: " + Environment.NewLine;
                        rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                        rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                        rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                        rtbMessages.Text += Environment.NewLine;

                        player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                        player.Gold += newLocation.QuestAvailableHere.RewardGold;

                        player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);
                        player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                    }
                }
            }
            else
            {
                //Player recieves quest
                rtbMessages.Text += "You receive the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine;
                rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                rtbMessages.Text += "To complete it, return with:" + Environment.NewLine;
                foreach (FinishedQuest finishedQuest in newLocation.QuestAvailableHere.FinishedQuests)
                {
                    if (finishedQuest.Quantity == 1)
                    {
                        rtbMessages.Text += finishedQuest.Quantity.ToString() + " " + finishedQuest.Item.Name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += finishedQuest.Quantity.ToString() + " " + finishedQuest.Item.NamePlural + Environment.NewLine;
                    }
                }
                rtbMessages.Text += Environment.NewLine;

                // Add the quest to the player's quest list
                player.Quests.Add(new AdventureQuest(newLocation.QuestAvailableHere));
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            Weapon currentWeapon = (Weapon)comboBoxWeapons.SelectedItem;
            int damageToMonster = RandomNumGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            currentMonster.CurrentPoints -= damageToMonster;
            rtbMessages.Text += "You hit the " + currentMonster.Name + " for " + damageToMonster.ToString() + " points." + Environment.NewLine;

            if (currentMonster.CurrentPoints <= 0)
            {
                //Gremlin is dead
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "You defeated the " + currentMonster.Name + Environment.NewLine;
                
                player.ExperiencePoints += currentMonster.RewardExperiencePoints;
                rtbMessages.Text += "You receive " + currentMonster.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                
                player.Gold += currentMonster.RewardGold;
                rtbMessages.Text += "You receive " + currentMonster.RewardGold.ToString() + " gold" + Environment.NewLine;
                
                List<Inventory> lootedItems = new List<Inventory>();

                //Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (Treasure lootItem in currentMonster.TreasureChest)
                {
                    if (RandomNumGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new Inventory(lootItem.Item, 1));
                    }
                }

                //If no items were randomly selected, then add the default loot item(s).
                if (lootedItems.Count == 0)
                {
                    foreach (Treasure lootItem in currentMonster.TreasureChest)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new Inventory(lootItem.Item, 1));
                        }
                    }
                }

                // Add the looted items to the player's inventory
                foreach (Inventory inventoryItem in lootedItems)
                {
                    player.AddItemToInventory(inventoryItem.Item);

                    if (inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Item.Name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Item.NamePlural + Environment.NewLine;
                    }
                }

                // Refresh player information and inventory controls
                lblPoints.Text = player.CurrentPoints.ToString();
                lblGold.Text = player.Gold.ToString();
                lblExperience.Text = player.ExperiencePoints.ToString();
                lblLevel.Text = player.Level.ToString();

                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();

                // Add a blank line to the messages box, just for appearance.
                rtbMessages.Text += Environment.NewLine;

                //Move player to current location (to heal player and create a new monster to fight)
                MoveTo(player.CurrentLocation);
            }
            else
            {
                // Monster is still alive

                // Determine the amount of damage the monster does to the player
                int damageToPlayer = RandomNumGenerator.NumberBetween(0, currentMonster.MaximumDamage);

                // Display message
                rtbMessages.Text += "The " + currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;

                // Subtract damage from player
                player.CurrentPoints -= damageToPlayer;

                // Refresh player data in UI
                lblPoints.Text = player.CurrentPoints.ToString();

                if (player.CurrentPoints <= 0)
                {
                    // Display message
                    rtbMessages.Text += "The " + currentMonster.Name + " killed you." + Environment.NewLine;

                    // Move player to "Home"
                    MoveTo(Swamp.LocationByID(Swamp.LOCATION_ID_HOME));
                }
            }


        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        { 
            //Get elixir
            Elixir potion = (Elixir)comboBoxPotions.SelectedItem;
            
            player.CurrentPoints = (player.CurrentPoints + potion.AmountToHeal);
            
            if (player.CurrentPoints > player.MaximumPoints)
            {
                player.CurrentPoints = player.MaximumPoints;
            }

            //Remove elixir inventory
            foreach (Inventory item in player.Inventory)
            {
                if (item.Item.ID == potion.ID)
                {
                    item.Quantity--;
                    break;
                }
            }

            //Display message
            rtbMessages.Text += "You drink a " + potion.Name + Environment.NewLine;

            //Gremlin attacks
            
            int damageToPlayer = RandomNumGenerator.NumberBetween(0, currentMonster.MaximumDamage);
            
            rtbMessages.Text += "The " + currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine;
            
            player.CurrentPoints -= damageToPlayer;

            if (player.CurrentPoints <= 0)
            {
                rtbMessages.Text += "The " + currentMonster.Name + " killed you." + Environment.NewLine;

                //Move player back home
                MoveTo(Swamp.LocationByID(Swamp.LOCATION_ID_HOME));
            }

            //Refresh data
            lblPoints.Text = player.CurrentPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }

    }
}