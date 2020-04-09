using System;
using System.Collections.Generic;

namespace CovidBot.Luis.Entity
{
    public class CompositeEntity :  Entity
    {
        public IEnumerable<CompositeChild> Children { get; set; }
    }
}
