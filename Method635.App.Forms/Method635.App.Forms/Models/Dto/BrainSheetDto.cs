using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Forms.Dto
{
    public class BrainSheetDto
    {
        [JsonProperty("nrOfSheet")]
        public int NrOfSheet { get; set; }

        [JsonProperty("brainwaves")]
        public List<BrainWaveDto> BrainWaves { get; set; }
    }
}
