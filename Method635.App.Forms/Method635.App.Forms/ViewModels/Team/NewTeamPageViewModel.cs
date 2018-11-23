using Method635.App.Forms.BusinessModels;
using Method635.App.Forms.Context;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;

namespace Method635.App.Forms.ViewModels.Team
{
    public class NewTeamPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly BrainstormingContext _context;
        public NewTeamPageViewModel(INavigationService navigationService, BrainstormingContext context)
        {
            this._navigationService = navigationService;
            this._context = context;
            this.CreateTeamCommand = new DelegateCommand(CreateTeam);
        }

        private void CreateTeam()
        {
            var newTeam = new BrainstormingTeam()
            {
                Name = TeamName,
                NrOfParticipants = TeamSize,
                Purpose = Purpose,
                Moderator = new Moderator(_context.CurrentParticipant)
            };
            var newTeamWithId = new TeamRestResolver().CreateBrainstormingTeam(newTeam);
            if (string.IsNullOrEmpty(newTeamWithId.Id))
            {
                Console.WriteLine("There was an error creating the new brainstorming team.. No Id returned");
            }
            this._context.CurrentBrainstormingTeam = newTeamWithId;
            this._navigationService.NavigateAsync("MainPage/NavigationPage/InviteTeamPage");
        }

        public string TeamName { get; set; } = string.Empty;
        public int TeamSize { get; set; }
        public string Purpose { get; set; } = string.Empty;


        public DelegateCommand CreateTeamCommand { get; }
    }
}
