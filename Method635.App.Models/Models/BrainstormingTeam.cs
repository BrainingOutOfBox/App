using System.Collections.Generic;

namespace Method635.App.Models
{
    public class BrainstormingTeam
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public int NrOfParticipants { get; set; }
        public int CurrentNrOfParticipants { get; set; }
        public List<Participant> Participants { get; set; }
        public Moderator Moderator { get; set; }
    }
}

