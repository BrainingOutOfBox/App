using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using Method635.App.Logging;
using Method635.App.Forms.Services;
using Method635.App.Forms.Resources;
using Method635.App.BL.Context;
using Method635.App.BL.Interfaces;
using System.Threading.Tasks;
using Prism.Navigation;
using Method635.App.Forms.Models;

namespace Method635.App.Forms.ViewModels.Team
{
    public class TeamPageViewModel : BindableBase, INavigatedAware
	{
        private readonly IUiNavigationService _navigationService;
        private readonly ITeamService _teamService;
        private readonly BrainstormingContext _context;

        private readonly ILogger _logger;

        public TeamPageViewModel(
            ILogger logger,
            IUiNavigationService navigationService, 
            ITeamService teamService,
            BrainstormingContext context)
        {
            _logger = logger;
            _navigationService = navigationService;
            _teamService = teamService;
            _context = context;
            
            TeamList = FillTeamList();
            if (TeamList.Any() && _context.CurrentBrainstormingTeam == null)
            {
                SelectedTeam = TeamList[0];
                _context.CurrentBrainstormingTeam = SelectedTeam;
            }
            SelectTeamCommand = new DelegateCommand(SelectTeam);
            CreateTeamCommand = new DelegateCommand(async ()=> await CreateTeam());
            JoinTeamCommand = new DelegateCommand(async ()=> await JoinTeam());
            ShowQrCodeCommand = new DelegateCommand<BrainstormingTeamModel>(ShowQrCode);
            LeaveTeamCommand = new DelegateCommand<BrainstormingTeamModel>(LeaveTeam);
            RefreshCommand = new DelegateCommand(async()=>await Task.Run(RefreshTeamList));
        }

        private void ShowQrCode(BrainstormingTeamModel team)
        {
            _context.CurrentBrainstormingTeam = team;
            _navigationService.NavigateToInviteTeam();
        }

        private async Task RefreshTeamList()
        {
            IsRefreshing = true;
            TeamList = await Task.Run(() => FillTeamList());
            IsRefreshing = false;
        }

        private void LeaveTeam(BrainstormingTeamModel team)
        {
            _logger.Info("Leaving team...");
        }

        private async Task JoinTeam()
        {
            await _navigationService.NavigateToJoinTeam();
        }

        private async Task CreateTeam()
        {
            await _navigationService.NavigateToCreateTeam();
        }

        private void SelectTeam()
        {
            _context.CurrentBrainstormingTeam = _selectedTeam;
            _navigationService.NavigateToBrainstormingListTab();
        }

        private List<BrainstormingTeamModel> FillTeamList()
        {
            var teamList = _teamService.GetTeamsByUserName(_context.CurrentParticipant.UserName)
                .Select((t) => new BrainstormingTeamModel(t)
            {
                IsModerator = _context.CurrentParticipant.UserName.Equals(t.Moderator.UserName)
            });
            HasTeam = teamList.Any();
            return teamList.ToList();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            TeamList = FillTeamList();
        }

        private List<BrainstormingTeamModel> _teamList;
        public List<BrainstormingTeamModel> TeamList
        {
            get => _teamList; private set
            {
                SetProperty(ref _teamList, value);
            }
        }
        public DelegateCommand SelectTeamCommand { get; }
        public DelegateCommand CreateTeamCommand { get; }
        public DelegateCommand JoinTeamCommand { get; }
        public DelegateCommand<BrainstormingTeamModel> ShowQrCodeCommand { get; }
        public DelegateCommand<BrainstormingTeamModel> LeaveTeamCommand { get; }
        public DelegateCommand RefreshCommand { get; }

        private BrainstormingTeamModel _selectedTeam;
        public BrainstormingTeamModel SelectedTeam {
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
        private bool _isModerator;
        public bool IsModerator { get => _isModerator; set => SetProperty(ref _isModerator, value); }

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set => SetProperty(ref _isRefreshing, value); }
        public string Title => AppResources.MyTeams;
	}
}
