using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
