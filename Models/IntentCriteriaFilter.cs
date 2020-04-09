using System;
namespace CoronaBot.Models
{
    public class IntentCriteriaFilter
    {
        public string Question { get; set; }
        public bool ByQuestion { get; set; }
        public string Answer { get; set; } 
        public bool ByAnswer { get; set; }
        public string Category { get; set; }
        public bool ByCategory { get; set; }
        public string SubCategory { get; set; }
        public bool BySubCategory { get; set; }
        public string Intent { get; set; }
        public bool ByIntent { get; set; }
    }
}
