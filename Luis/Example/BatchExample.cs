using System;
using CovidBot.Luis.Common;
using CovidBot.Luis.Utterance;
namespace CovidBot.Luis.Example
{
    public class BatchExample
    {
        public CovidBot.Luis.Utterance.Utterance Value { get; set; }
        public bool HasError { get; set; }
        public Error Error { get; set; }
    }
}
