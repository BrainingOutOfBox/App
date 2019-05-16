using Method635.App.Models;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Forms.Services;
using Method635.App.BL.Context;
using Method635.App.BL.Interfaces;
using Method635.App.Forms.Resources;
using System;

namespace Method635.App.Forms.ViewModels.Team
{
    public class NewTeamPageViewModel : BindableBase
    {
        private readonly IUiNavigationService _navigationService;
        private readonly ITeamService _teamService;
        private readonly BrainstormingContext _context;

        private readonly ILogger _logger;

        public NewTeamPageViewModel(
            ILogger logger,
            IUiNavigationService navigationService,
            ITeamService teamService,
            BrainstormingContext context)
        {
            _logger = logger;
            _navigationService = navigationService;
            _teamService = teamService;
            _context = context;
            CreateTeamCommand = new DelegateCommand(AddTeam);
        }

        private void AddTeam()
        {
            if (!CheckInput())
            {
                _logger.Error($"Invalid input to create team");
                return;
            }
            var newTeam = new BrainstormingTeam()
            {
                Name = TeamName,
                NrOfParticipants = _teamSize,
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

        private bool CheckInput()
        {
            if (!int.TryParse(TeamSizeString, out int teamSize))
            {
                ErrorText = AppResources.UseNumbersInFields;
                HasError = true;
                return false;
            }
            _teamSize = teamSize;
            return true;
        }

        public string TeamName { get; set; } = string.Empty;
        private int _teamSize;
        public string TeamSizeString { get; set; }
        public string Purpose { get; set; } = string.Empty;
        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }
        
        public DelegateCommand CreateTeamCommand { get; }
    }
}
