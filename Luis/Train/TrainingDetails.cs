using System;
namespace CovidBot.Luis.Train
{
    public class TrainingDetails
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int ExampleCount { get; set; }
        public DateTime? TrainingDateTime { get; set; }
        public string FailureReason { get; set; }
    }
}
