using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Dal.Config
{
    public class TeamEndpoints
    {
        [JsonProperty("team")]
        public string TeamEndpoint { get; set; }
        [JsonProperty("createTeam")]
        public string CreateEndpoint { get; set; }
        [JsonProperty("getTeam")]
        public string GetEndpoint { get; set; }
        [JsonProperty("getMyTeam")]
        public string GetAllEndpoint { get; set; }
        [JsonProperty("join")]
        public string JoinEndpoint { get; set; }
    }
}
