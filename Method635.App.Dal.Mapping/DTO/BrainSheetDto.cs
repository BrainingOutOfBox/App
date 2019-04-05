using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Method635.App.Dal.Mapping
{
    public class BrainSheet
    {
        [JsonProperty("nrOfSheet")]
        public int NrOfSheet { get; set; }

        [JsonProperty("brainwaves")]
        public List<BrainWave> BrainWaves { get; set; }
    }
}
