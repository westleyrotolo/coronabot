﻿using System;
namespace CovidBot.Luis.Entity
{
    public class EntityLabel
    {
        public string EntityName { get; set; }
        public int StartCharIndex { get; set; }
        public int EndCharIndex { get; set; }
    }
}
