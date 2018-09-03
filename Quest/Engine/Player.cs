using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : Creature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }
        public List<Inventory> Inventory { get; set; }
        public List<AdventureQuest> Quests { get; set; }

        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;

            Inventory = new List<Inventory>();
            Quests = new List<AdventureQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
            {
                // There is no required item for this location, so return "true"
                return true;
            }

            // See if the player has the required item in their inventory
            return Inventory.Exists(x => x.Item.ID == location.ItemRequiredToEnter.ID);
        }

        public bool HasThisQuest(Quest quest)
        {
            return Quests.Exists(x => x.Quest.ID == quest.ID);
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach (AdventureQuest playerQuest in Quests)
            {
                if (playerQuest.Quest.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            // See if the player has all the items needed to complete the quest here
            foreach (FinishedQuest finishedQuest in quest.FinishedQuests)
            {
                // Check each item in the player's inventory, to see if they have it, and enough of it
                if (!Inventory.Exists(i => i.Item.ID == finishedQuest.Item.ID && i.Quantity >= finishedQuest.Quantity))
                {
                    return false;
                }
            }

            // If we got here, then the player must have all the required items, and enough of them, to complete the quest.
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (FinishedQuest finishedQuest in quest.FinishedQuests)
            {
                Inventory item = Inventory.SingleOrDefault(i => i.Item.ID == finishedQuest.Item.ID);

                if (item != null)
                {
                    // Subtract the quantity from the player's inventory that was needed to complete the quest
                    item.Quantity -= finishedQuest.Quantity;
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (Inventory item in Inventory)
            {
                if (item.Item.ID == itemToAdd.ID)
                {
                    // They have the item in their inventory, so increase the quantity by one
                    item.Quantity++;

                    return; // We added the item, and are done, so get out of this function
                }
            }

            // They didn't have the item, so add it to their inventory, with a quantity of 1
            Inventory.Add(new Inventory(itemToAdd, 1));
        }

        public void MarkQuestCompleted(Quest quest)
        {
            // Find the quest in the player's quest list
            AdventureQuest playerQuest = Quests.SingleOrDefault(pq => pq.Quest.ID == quest.ID);

            if (playerQuest != null)
            {
                playerQuest.IsCompleted = true;
            }
        }
    }
}