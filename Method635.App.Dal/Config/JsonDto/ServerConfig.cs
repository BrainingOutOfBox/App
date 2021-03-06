﻿using Method635.App.Dal.Config.JsonDto;
using Newtonsoft.Json;

namespace Method635.App.Dal.Config
{
    public class ServerConfig : IServerConfig
    {
        [JsonProperty("server")]
        public Server Server { get; set; }
        [JsonProperty("brainstorming-endpoints")]
        public BrainstormingEndpoints BrainstormingEndpoints { get; set; }
        [JsonProperty("participant-endpoints")]
        public ParticipantEndpoints ParticipantEndpoints { get; set; }
        [JsonProperty("team-endpoints")]
        public TeamEndpoints TeamEndpoints { get; set; }
        [JsonProperty("files-endpoints")]
        public FileEndpoints FileEndpoints {get;set;}
        [JsonProperty("pattern-endpoints")]
        public PatternEndpoints PatternEndpoints { get; set; }

    }
    public class Server
    {
        [JsonProperty("hostname")]
        public string HostName { get; set; }
        [JsonProperty("port")]
        public string Port { get; set; }
    }
}
