using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Forms.Dto
{
    public class BrainstormingFindingDto
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

        [JsonProperty("currentRound")]
        public int CurrentRound { get; set; }

        [JsonProperty("brainsheets")]
        public List<BrainSheetDto> BrainSheets { get; set; }
    }
}
