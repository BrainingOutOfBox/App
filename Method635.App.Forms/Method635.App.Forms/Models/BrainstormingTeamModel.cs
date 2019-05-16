using Method635.App.Models;

namespace Method635.App.Forms.Models
{
    public class BrainstormingTeamModel : BrainstormingTeam
    {
        public BrainstormingTeamModel(BrainstormingTeam t)
        {
            Id = t.Id;
            Name = t.Name;
            Moderator = t.Moderator;
            NrOfParticipants = t.NrOfParticipants;
            CurrentNrOfParticipants = t.CurrentNrOfParticipants;
            Participants = t.Participants;
            Purpose = t.Purpose;
        }
        private bool _isModerator;
        public bool IsModerator { get => _isModerator; set => SetProperty(ref _isModerator, value); }
    }
}
