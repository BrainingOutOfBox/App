using Method635.App.Forms.Dto;
using Newtonsoft.Json;

namespace Method635.App.Forms.RestAccess.ResponseModel
{
    public class RestLoginResponseDto
    {
        [JsonProperty("participant")]
        public ParticipantDto Participant { get; set; }
        [JsonProperty("jwt_token")]
        public string JwtToken { get; set; }
    }
}
