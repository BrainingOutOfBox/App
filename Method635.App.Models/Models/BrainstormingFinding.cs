using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Models
{
    public class BrainstormingFinding : PropertyChangedBase
    {
        [JsonProperty("identifier")]
        public string Id { get; set; }

        [JsonProperty("brainstormingTeam")]
        public string TeamId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("problemDescription")]
        public string ProblemDescription { get; set; }

        [JsonProperty("nrOfIdeas")]
        public int NrOfIdeas { get; set; }

        [JsonProperty("baseRoundTime")]
        public int BaseRoundTime { get; set; }


        private int _currentRound;
        [JsonProperty("currentRound")]
        public int CurrentRound { get=>_currentRound; set=>SetProperty(ref _currentRound, value); }

        [JsonProperty("brainsheets")]
        public List<BrainSheet> BrainSheets { get; set; }
    }
}
