using System;
namespace CoronaBot.Models
{
    public class FeedSearch
    {
        public string[] Titles { get; set; }
        int Take { get; set; }
        int Skip { get; set; }
    }
}
