using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class AdventureQuest
    {
        public Quest Quest { get; set; }
        public bool IsCompleted { get; set; }

        public AdventureQuest(Quest quest)
        {
            Quest = quest;
            IsCompleted = false;
        }
    }
}