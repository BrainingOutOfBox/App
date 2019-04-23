using Newtonsoft.Json;

namespace Method635.App.Dal.Config.JsonDto
{
    public class TeamEndpoints
    {
        [JsonProperty("team")]
        public string TeamEndpoint { get; set; }
        [JsonProperty("createTeam")]
        public string CreateEndpoint { get; set; }
        [JsonProperty("getTeam")]
        public string GetEndpoint { get; set; }
        [JsonProperty("getTeams")]
        public string GetAllEndpoint { get; set; }
        [JsonProperty("join")]
        public string JoinEndpoint { get; set; }
    }
}
