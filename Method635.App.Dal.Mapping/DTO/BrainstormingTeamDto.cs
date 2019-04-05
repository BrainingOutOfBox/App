using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Models
{
    public class BrainstormingTeam
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
        public List<Participant> Participants { get; set; }
        [JsonProperty("moderator")]
        public Moderator Moderator { get; set; }
    }
}

