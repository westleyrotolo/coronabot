using System;
namespace CovidBot.Luis.Entity
{
    public class ReviewEntityLabel
    {
        public string EntityName { get; set; }
        public int StartTokenIndex { get; set; }
        public int EndTokenIndex { get; set; }
    }
}
