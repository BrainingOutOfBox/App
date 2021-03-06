﻿using Newtonsoft.Json;

namespace Method635.App.Dal.Mapping
{
    public class ParticipantDto
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("firstname")]
        public string FirstName { get; set; }
        [JsonProperty("lastname")]
        public string LastName { get; set; }
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}
