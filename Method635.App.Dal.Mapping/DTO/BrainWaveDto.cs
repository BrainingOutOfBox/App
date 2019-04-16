using Method635.App.Dal.Mapping.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Dal.Mapping
{
    public class BrainWaveDto
    {
        [JsonProperty("nrOfBrainwave")]
        public int NrOfBrainWave { get; set; }

        [JsonProperty("ideas")]
        public List<IdeaDto> Ideas { get; set; }
    }
}
