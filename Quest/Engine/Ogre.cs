using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Ogre : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public List<Treasure> TreasureChest { get; set; }

        public Ogre(int id, string name, int maximumDamage, int rewardExperiencePoints, int rewardGold, int currentPoints, int maximumPoints) : base(currentPoints, maximumPoints)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            TreasureChest = new List<Treasure>();
        }
    }
}