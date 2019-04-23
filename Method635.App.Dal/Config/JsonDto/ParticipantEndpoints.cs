using Newtonsoft.Json;

namespace Method635.App.Dal.Config.JsonDto
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
