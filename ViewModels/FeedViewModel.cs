using System;
namespace CovidBot.ViewModels
{
    public class FeedViewModel
    {

        public string Link { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Media { get; set; }
        public DateTime PubDate { get; set; }
        public string Author { get; set; }
    }
}
