using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Treasure
    {
        public Item Item { get; set; }
        public int DropPercentage { get; set; }
        public bool IsDefaultItem { get; set; }

        public Treasure(Item item, int dropPercentage, bool isDefaultItem)
        {
            Item = item;
            DropPercentage = dropPercentage;
            IsDefaultItem = isDefaultItem;
        }
    }
}