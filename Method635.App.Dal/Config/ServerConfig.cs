using Newtonsoft.Json;

namespace Method635.App.Dal.Config
{
    internal class ServerConfig
    {
        [JsonProperty("server")]
        public Server Server { get; set; }
        [JsonProperty("brainstorming-endpoints")]
        public BrainstormingEndpoints BrainstormingEndpoints { get; set; }
        [JsonProperty("participant-endpoints")]
        public ParticipantEndpoints ParticipantEndpoints { get; set; }

    }
    internal class Server
    {
        [JsonProperty("hostname")]
        public string HostName { get; set; }
        [JsonProperty("port")]
        public string Port { get; set; }
    }
}
