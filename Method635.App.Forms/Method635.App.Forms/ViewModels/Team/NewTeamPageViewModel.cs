using Method635.App.Models;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Forms.Services;
using Method635.App.BL.Context;
using Method635.App.BL.Interfaces;

namespace Method635.App.Forms.ViewModels.Team
{
    public class NewTeamPageViewModel : BindableBase
    {
        private readonly IUiNavigationService _navigationService;
        private readonly ITeamService _teamService;
        private readonly BrainstormingContext _context;

        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public NewTeamPageViewModel(IUiNavigationService navigationService,
            ITeamService teamService,
            BrainstormingContext context)
        {
            _navigationService = navigationService;
            _teamService = teamService;
            _context = context;
            CreateTeamCommand = new DelegateCommand(AddTeam);
        }

        private void AddTeam()
        {
            var newTeam = new BrainstormingTeam()
            {
                Name = TeamName,
                NrOfParticipants = TeamSize,
                Purpose = Purpose,
                Moderator = new Moderator(_context.CurrentParticipant)
            };
            var newTeamWithId = _teamService.AddTeam(newTeam);
            if (string.IsNullOrEmpty(newTeamWithId.Id))
            {
                _logger.Error("There was an error creating the new brainstorming team.. No Id returned");
            }
            _context.CurrentBrainstormingTeam = newTeamWithId;
            _navigationService.NavigateToInviteTeam();
        }

        public string TeamName { get; set; } = string.Empty;
        public int TeamSize { get; set; }
        public string Purpose { get; set; } = string.Empty;


        public DelegateCommand CreateTeamCommand { get; }
    }
}
