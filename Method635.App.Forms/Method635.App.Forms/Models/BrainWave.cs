using Newtonsoft.Json;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Method635.App.Forms.Models
{
    public class BrainWave : BindableBase
    {
        [JsonProperty("nrOfBrainwave")]
        public int NrOfBrainWave { get; set; }

        private List<TextIdea> _ideas;
        [JsonProperty("ideas")]
        public List<TextIdea> Ideas { get=>_ideas; set=>SetProperty(ref _ideas, value); }
    }
}
