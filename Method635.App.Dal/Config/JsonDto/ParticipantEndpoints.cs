using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Dal.Config
{
    public class ParticipantEndpoints
    {
        [JsonProperty("participant")]
        public string ParticipantEndpoint { get; set; }
        [JsonProperty("register")]
        public string RegisterEndpoint { get; set; }
        [JsonProperty("login")]
        public string LoginEndpoint { get; set; }
    }
}
