using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
