using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class AdventureQuest
    {
        public Quest Details { get; set; }
        public bool IsCompleted { get; set; }

        public AdventureQuest(Quest details)
        {
            Details = details;
            IsCompleted = false;
        }
    }
}