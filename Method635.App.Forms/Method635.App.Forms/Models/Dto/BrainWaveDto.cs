using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Forms.Dto
{
    public class BrainWaveDto
    {
        [JsonProperty("nrOfBrainwave")]
        public int NrOfBrainWave { get; set; }

        [JsonProperty("ideas")]
        public List<TextIdeaDto> Ideas { get; set; }
    }
}
