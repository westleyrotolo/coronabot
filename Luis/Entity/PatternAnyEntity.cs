using System;
using System.Collections.Generic;

namespace CovidBot.Luis.Entity
{
    public class PatternAnyEntity : Entity
    {
        public IEnumerable<PatternAnyItem> ExplicityList { get; set; }
    }
}
