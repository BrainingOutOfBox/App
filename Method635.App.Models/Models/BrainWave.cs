using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Models
{
    public class BrainWave : PropertyChangedBase
    {
        [JsonProperty("nrOfBrainwave")]
        public int NrOfBrainWave { get; set; }

        private List<TextIdea> _ideas;
        [JsonProperty("ideas")]
        public List<TextIdea> Ideas { get=>_ideas; set=>SetProperty(ref _ideas, value); }
    }
}
