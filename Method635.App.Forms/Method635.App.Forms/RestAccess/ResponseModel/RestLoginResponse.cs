using Method635.App.Forms.Models;
using Newtonsoft.Json;

namespace Method635.App.Forms.RestAccess.ResponseModel
{
    public class RestLoginResponse
    {
        [JsonProperty("participant")]
        public Participant Participant { get; set; }
        [JsonProperty("jwt_token")]
        public string JwtToken { get; set; }
    }
}
