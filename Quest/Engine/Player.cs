using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public List<Inventory> Inventory { get; set; }
        public List<AdventureQuest> Quests { get; set; }

        public Player(int currentPoints, int maximumPoints, int gold, int experiencePoints, int level) : base(currentPoints, maximumPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            Inventory = new List<Inventory>();
            Quests = new List<AdventureQuest>();
        }
    }
}
