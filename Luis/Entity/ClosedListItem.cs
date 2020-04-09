using System;
using System.Collections.Generic;

namespace CovidBot.Luis.Entity
{
    public class ClosedListItem
    {
        public int Id { get; set; }
        public string CanonicalForm { get; set; }
        public IEnumerable<string> List { get; set; }
    }
}
