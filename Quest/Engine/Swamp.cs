using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class Swamp
    {
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Gremlin> Gremlins = new List<Gremlin>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();

        public const int ITEM_ID_SLINGSHOT = 1;
        public const int ITEM_ID_SHOE = 2;
        public const int ITEM_ID_RING = 3;
        public const int ITEM_ID_DRAGON_SCALE = 4;
        public const int ITEM_ID_DRAGON_TOOTH = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_ELIXIR = 7;
        public const int ITEM_ID_BEJEWELED_EYE = 8;
        public const int ITEM_ID_GOLD_TOOTH = 9;
        public const int ITEM_ID_GUARD_PASS = 10;

        public const int MONSTER_ID_GNOME = 1;
        public const int MONSTER_ID_DRAGON = 2;
        public const int MONSTER_ID_SHREK = 3;

        public const int QUEST_ID_CLEAR_APOTHECARY = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_APOTHECARY = 4;
        public const int LOCATION_ID_APOTHECARY_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SHREK_FOREST = 9;

        static Swamp()
        {
            PopulateItems();
            PopulateGremlins();
            PopulateQuests();
            PopulateLocations();
        }

        private static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_SLINGSHOT, "Slingshot", "Slingshots", 0, 5));
            Items.Add(new Item(ITEM_ID_SHOE, "Leather shoe", "Leather shoes"));
            Items.Add(new Item(ITEM_ID_RING, "Silver ring", "Silver rings"));
            Items.Add(new Item(ITEM_ID_DRAGON_SCALE, "Dragon scale", "Dragon scales"));
            Items.Add(new Item(ITEM_ID_DRAGON_TOOTH, "Dragon tooth", "Dragon tooth"));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 10));
            Items.Add(new Elixir(ITEM_ID_HEALING_ELIXIR, "Healing elixir", "Healing elixirs", 5));
            Items.Add(new Item(ITEM_ID_BEJEWELED_EYE, "Bejeweled eye", "Bejeweled eyes"));
            Items.Add(new Item(ITEM_ID_GOLD_TOOTH, "Gold tooth", "Gold teeth"));
            Items.Add(new Item(ITEM_ID_GUARD_PASS, "Guard pass", "Guard passes"));
        }

        private static void PopulateGremlins()
        {
            Gremlin gnome = new Gremlin(MONSTER_ID_GNOME, "Nefarious Garden Gnome", 5, 3, 10, 3, 3);
            gnome.TreasureChest.Add(new Treasure(ItemByID(ITEM_ID_SHOE), 75, false));
            gnome.TreasureChest.Add(new Treasure(ItemByID(ITEM_ID_RING), 75, true));

            Gremlin dragon = new Gremlin(MONSTER_ID_DRAGON, "Sly Dragon", 5, 3, 10, 3, 3);
            dragon.TreasureChest.Add(new Treasure(ItemByID(ITEM_ID_DRAGON_SCALE), 75, false));
            dragon.TreasureChest.Add(new Treasure(ItemByID(ITEM_ID_DRAGON_TOOTH), 75, true));

            Gremlin shrek = new Gremlin(MONSTER_ID_SHREK, "Shrek", 20, 5, 50, 10, 10);
            shrek.TreasureChest.Add(new Treasure(ItemByID(ITEM_ID_BEJEWELED_EYE), 75, true));
            shrek.TreasureChest.Add(new Treasure(ItemByID(ITEM_ID_GOLD_TOOTH), 25, false));

            Gremlins.Add(gnome);
            Gremlins.Add(dragon);
            Gremlins.Add(shrek);
        }

        private static void PopulateQuests()
        {
            Quest clearApothecaryGarden =
                new Quest(
                    QUEST_ID_CLEAR_APOTHECARY,
                    "Clear the apothecary garden",
                    "Kill nefarious gnomes in the apothecary garden and bring back 3 leather shoes. You will receive an elixir and 10 gold pieces.", 20, 10);

            clearApothecaryGarden.FinishedQuests.Add(new FinishedQuest(ItemByID(ITEM_ID_SHOE), 3));

            clearApothecaryGarden.RewardItem = ItemByID(ITEM_ID_HEALING_ELIXIR);

            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "Clear the farmer's field",
                    "Kill dragons in the farmer's field and bring back 3 dragon scales. You will receive an adventurer's pass and 20 gold pieces.", 20, 20);

            clearFarmersField.FinishedQuests.Add(new FinishedQuest(ItemByID(ITEM_ID_DRAGON_SCALE), 3));

            clearFarmersField.RewardItem = ItemByID(ITEM_ID_GUARD_PASS);

            Quests.Add(clearApothecaryGarden);
            Quests.Add(clearFarmersField);
        }

        private static void PopulateLocations()
        {
            //Craft locations
            Location home = new Location(LOCATION_ID_HOME, "Home", "Welcome back to your lovely home!");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town square", "Bright sunny day out...");

            Location apothecary = new Location(LOCATION_ID_APOTHECARY, "Apothecary", "There are many strange vials on the counter.");
            apothecary.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_APOTHECARY);

            Location alchemistsGarden = new Location(LOCATION_ID_APOTHECARY_GARDEN, "Garden", "Lots of interesting plants are growing here.");
            alchemistsGarden.GremlinLivingHere = GremlinByID(MONSTER_ID_GNOME);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "There is a farmer tending to crops beside the farmhouse.");
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Farmer's field", "You see rows of vegetables growing here.");
            farmersField.GremlinLivingHere = GremlinByID(MONSTER_ID_DRAGON);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard post", "There is a big burly-looking guard here.", ItemByID(ITEM_ID_GUARD_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "A cobblestone bridge crosses a swift murky river.");

            Location shrekForest = new Location(LOCATION_ID_SHREK_FOREST, "Forest", "An eerie vibe permeates the low-hanging trees");
            shrekForest.GremlinLivingHere = GremlinByID(MONSTER_ID_SHREK);

            //Map locations
            home.LocationToNorth = townSquare;

            townSquare.LocationToNorth = apothecary;
            townSquare.LocationToSouth = home;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToWest = farmhouse;

            farmhouse.LocationToEast = townSquare;
            farmhouse.LocationToWest = farmersField;

            farmersField.LocationToEast = farmhouse;

            apothecary.LocationToSouth = townSquare;
            apothecary.LocationToNorth = alchemistsGarden;

            alchemistsGarden.LocationToSouth = apothecary;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = townSquare;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = shrekForest;

            shrekForest.LocationToWest = bridge;

            //Add locations
            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(guardPost);
            Locations.Add(apothecary);
            Locations.Add(alchemistsGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(bridge);
            Locations.Add(shrekForest);
        }

        public static Item ItemByID(int id)
        {
            return Items.Where(item => item.ID == id).FirstOrDefault();
        }

        public static Gremlin GremlinByID(int id)
        {
            return Gremlins.Where(ogre => ogre.ID == id).FirstOrDefault();
        }

        public static Quest QuestByID(int id)
        {
            return Quests.Where(quest => quest.ID == id).FirstOrDefault();
        }

        public static Location LocationByID(int id)
        {
            return Locations.Where(location => location.ID == id).FirstOrDefault();
        }
    }
}