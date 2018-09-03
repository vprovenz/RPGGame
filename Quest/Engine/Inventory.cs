using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Inventory
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }

        public Inventory(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}