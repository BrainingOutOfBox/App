using Newtonsoft.Json;

namespace Method635.App.Forms.Models
{
    public class BrainstormingFinding
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("problemDescription")]
        public string ProblemDescription { get; set; }

        [JsonProperty("nrOfIdeas")]
        public int NrOfIdeas { get; set; }

        [JsonProperty("baseRoundTime")]
        public int BaseRoundTime { get; set; }
    }
}
