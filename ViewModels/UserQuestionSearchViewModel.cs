using System;
namespace CoronaBot.ViewModels
{
    public class UserQuestionSearchViewModel
    {
        public int ItemPerPage { get; set; }
        public int Page { get; set; }
        public bool ByIntent { get; set; }
        public string Intent { get; set; }
        public bool ByUser { get; set; }
        public string User { get; set; }
        public bool ByQuestion { get; set; }
        public string Question { get; set; }
    }
}
