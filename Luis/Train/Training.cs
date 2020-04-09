using System;
using Newtonsoft.Json;

namespace CovidBot.Luis.Train
{
    public class Training
    {
        public string ModelId { get; set; }

        [JsonProperty("details")]
        public TrainingDetails Details { get; set; }
    }
}
