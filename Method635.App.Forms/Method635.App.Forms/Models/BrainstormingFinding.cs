using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Forms.Models
{
    public class BrainstormingFinding
    {
        [JsonProperty("identifier")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("problemDescription")]
        public string ProblemDescription { get; set; }

        [JsonProperty("nrOfIdeas")]
        public int NrOfIdeas { get; set; }

        [JsonProperty("baseRoundTime")]
        public int BaseRoundTime { get; set; }

        [JsonProperty("currentRound")]
        public int CurrentRound { get; set; }

        [JsonProperty("brainsheets")]
        public List<BrainSheet> BrainSheets { get; set; }
    }
}
