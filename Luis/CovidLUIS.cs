using System;
namespace CovidBot.Luis
{
    public class CovidLUIS
    {
        public string query { get; set; }
        public MyIntent[] intents { get; set; }
        public MyEntity[]entities { get; set; }
    }
    public class MyIntent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }
    public class  MyEntity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public float score { get; set; }
    }
}

