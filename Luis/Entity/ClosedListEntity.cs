using System;
using System.Collections.Generic;

namespace CovidBot.Luis.Entity
{
    public class ClosedListEntity : Entity
    {
        public IEnumerable<ClosedListItem> SubLists { get; set; }
    }
}
