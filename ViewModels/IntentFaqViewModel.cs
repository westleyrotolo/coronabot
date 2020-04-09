using System;
namespace CoronaBot.ViewModels
{
    public class IntentFaqViewModel
    {
        public int Id { get; set; }
        public string Intent { get; set; }
        public string Questions { get; set; }
        public string Answers { get; set; }
        public string Category {get;set;}
        public string SubCategory {get;set;}
        public string Filename { get; set; }
        public string Filepath { get; set; }
    }
}
