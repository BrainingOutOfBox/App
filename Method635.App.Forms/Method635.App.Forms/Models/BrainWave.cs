using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Forms.Models
{
    public class BrainWave
    {
        [JsonProperty("nrOfBrainwave")]
        public int NrOfBrainWave { get; set; }

        [JsonProperty("ideas")]
        public List<Idea> Ideas { get; set; }
    }
}
