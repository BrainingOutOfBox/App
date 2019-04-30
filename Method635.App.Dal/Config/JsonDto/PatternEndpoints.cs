using Newtonsoft.Json;

namespace Method635.App.Dal.Config.JsonDto
{
    public class PatternEndpoints
    {
        [JsonProperty("patterns")]
        public string PatternEndpoint { get; set; }
        [JsonProperty("getPatterns")]
        public string GetAllEndpoint { get; set; }
    }
}
