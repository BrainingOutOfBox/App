using Method635.App.Forms.Context;
using Method635.App.Models;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Forms.Services;
using Method635.App.Forms.Resources;

namespace Method635.App.Forms.ViewModels.Team
{
    public class TeamPageViewModel : BindableBase
	{
        private readonly IUiNavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;

        // Platform independent logger necessary, thus resolving from xf dependency service.
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public TeamPageViewModel(IUiNavigationService navigationService, IEventAggregator eventAggregator, BrainstormingContext context)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _context = context;

            TeamList = FillTeamList();
            if (TeamList.Any() && _context.CurrentBrainstormingTeam == null)
            {
                SelectedTeam = TeamList[0];
                _context.CurrentBrainstormingTeam = SelectedTeam;
            }
            SelectTeamCommand = new DelegateCommand(SelectTeam);
            CreateTeamCommand = new DelegateCommand(CreateTeam);
            JoinTeamCommand = new DelegateCommand(JoinTeam);
            LeaveTeamCommand = new DelegateCommand<BrainstormingTeam>(LeaveTeam);
        }

        private void LeaveTeam(BrainstormingTeam team)
        {
            _logger.Info("Leaving team...");
        }

        private async void JoinTeam()
        {
            await _navigationService.NavigateToJoinTeam();
        }

        private async void CreateTeam()
        {
            await _navigationService.NavigateToCreateTeam();
        }

        private void SelectTeam()
        {
            _context.CurrentBrainstormingTeam = _selectedTeam;
            _navigationService.NavigateToBrainstormingListTab();
            //_eventAggregator.GetEvent<RenderBrainstormingListEvent>().Publish();
        }

        private List<BrainstormingTeam> FillTeamList()
        {
            var teamList = new TeamRestResolver().GetMyBrainstormingTeams(_context.CurrentParticipant.UserName);
            HasTeam = teamList.Any();
            return teamList;
        }

        private List<BrainstormingTeam> _teamList;
        public List<BrainstormingTeam> TeamList
        {
            get => _teamList; private set
            {
                SetProperty(ref _teamList, value);
            }
        }
        public DelegateCommand SelectTeamCommand { get; }
        public DelegateCommand CreateTeamCommand { get; }
        public DelegateCommand JoinTeamCommand { get; }
        public DelegateCommand<BrainstormingTeam> LeaveTeamCommand { get; }

        private BrainstormingTeam _selectedTeam;
        public BrainstormingTeam SelectedTeam {
            get =>_selectedTeam;
            set
            {
                SetProperty(ref _selectedTeam, value);
            }
        }

        private bool _hasTeam;
        public bool HasTeam
        {
            get => _hasTeam;
            set => SetProperty(ref _hasTeam, value);
        }

        public string Title => AppResources.MyTeams;
	}
}
