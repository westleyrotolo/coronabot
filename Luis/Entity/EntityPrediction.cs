﻿using System;
namespace CovidBot.Luis.Entity
{
    public class EntityPrediction
    {
        public string EntityName { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string Phrase { get; set; }
    }
}
