using System;
using System.Collections.Generic;
using CovidBot.Luis.Entity;

namespace CovidBot.Luis.Example
{
    public class Example
    {
        public string Text { get; set; }
        public string IntentName { get; set; }
        public IEnumerable<EntityLabel> EntityLabels { get; set; }
    }
}
