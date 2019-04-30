using Newtonsoft.Json;

namespace Method635.App.Dal.Config.JsonDto
{
    public class BrainstormingEndpoints
    {
        [JsonProperty("findings")]
        public string FindingsEndpoint { get; set; }
        [JsonProperty("createFinding")]
        public string CreateEndpoint { get; set;}
        [JsonProperty("getFinding")]
        public string GetEndpoint { get; set; }
        [JsonProperty("getFindings")]
        public string GetAllEndpoint { get; set; }
        [JsonProperty("startFinding")]
        public string StartEndpoint { get; set; }
        [JsonProperty("remainingTime")]
        public string RemainingTimeEndpoint { get; set; }
        [JsonProperty("updateBrainsheet")]
        public string UpdateBrainsheetEndpoint { get; set; }

    }
}
