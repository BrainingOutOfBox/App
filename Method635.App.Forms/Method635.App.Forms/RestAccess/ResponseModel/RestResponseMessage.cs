using Newtonsoft.Json;

namespace Method635.App.Forms.RestAccess.ResponseModel
{
    public class RestResponseMessage
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
