using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Creature
    {
        public int CurrentPoints { get; set; }
        public int MaximumPoints { get; set; }

        public Creature(int currentPoints, int maximumPoints)
        {
            CurrentPoints = currentPoints;
            MaximumPoints = maximumPoints;
        }
    }
}
