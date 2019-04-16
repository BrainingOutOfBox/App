using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Dal.Mapping
{
    public class BrainstormingTeamDto
    {
        [JsonProperty("identifier")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("purpose")]
        public string Purpose { get; set; } = string.Empty;
        [JsonProperty("nrOfParticipants")]
        public int NrOfParticipants { get; set; }
        [JsonProperty("currentNrOfParticipants")]
        public int CurrentNrOfParticipants { get; set; }
        [JsonProperty("participants")]
        public List<ParticipantDto> Participants { get; set; }
        [JsonProperty("moderator")]
        public ModeratorDto Moderator { get; set; }
    }
}

