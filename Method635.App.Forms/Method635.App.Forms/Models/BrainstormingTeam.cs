using Newtonsoft.Json;
using System.Collections.Generic;

namespace Method635.App.Forms.Models
{
    public class BrainstormingTeam
    {
        [JsonProperty("identifier")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("purpose")]
        public string Purpose { get; set; }
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

