using Method635.App.Models;
using Prism.Mvvm;

namespace Method635.App.Forms.Context
{
    public class BrainstormingContext : BindableBase
    {
        public BrainstormingTeam CurrentBrainstormingTeam { get; set; }
        public Participant CurrentParticipant { get; set; }
        public string JwtToken { get; set; }

        private BrainstormingFinding _currentFinding;
        public BrainstormingFinding CurrentFinding { get => _currentFinding; set => SetProperty(ref _currentFinding, value); }
    }
}
