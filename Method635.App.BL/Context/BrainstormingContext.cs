using Method635.App.Models;

namespace Method635.App.BL.Context
{
    public class BrainstormingContext : PropertyChangedBase
    {
        public BrainstormingTeam CurrentBrainstormingTeam { get; set; }
        public Participant CurrentParticipant { get; set; }
        public string JwtToken { get; set; }

        private BrainstormingFinding _currentFinding;
        public BrainstormingFinding CurrentFinding { get => _currentFinding; set => SetProperty(ref _currentFinding, value); }
    }
}
