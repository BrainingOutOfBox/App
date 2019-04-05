using Method635.App.Dal.Mapping.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Dal.Mapping
{
    public class BrainWave
    {
        [JsonProperty("nrOfBrainwave")]
        public int NrOfBrainWave { get; set; }

        [JsonProperty("ideas")]
        public List<Idea> Ideas { get; set; }
    }
}
