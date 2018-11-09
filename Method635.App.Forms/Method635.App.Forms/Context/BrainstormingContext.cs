using Method635.App.Forms.Models;

namespace Method635.App.Forms.Context
{
    public class BrainstormingContext
    {
        public BrainstormingTeam CurrentBrainstormingTeam { get; set; }
        public Participant CurrentParticipant { get; set; }
        public BrainstormingFinding CurrentFinding { get; set; }
    }
}
