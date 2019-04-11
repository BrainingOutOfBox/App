using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Models
{
    public class BrainSheet
    {
        [JsonProperty("nrOfSheet")]
        public int NrOfSheet { get; set; }

        [JsonProperty("brainwaves")]
        public List<BrainWave> BrainWaves { get; set; }
    }
}
