﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class FinishedQuest
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }

        public FinishedQuest(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}