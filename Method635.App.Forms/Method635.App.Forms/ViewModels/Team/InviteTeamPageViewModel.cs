using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Method635.App.Forms.Services;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System.Timers;

namespace Method635.App.Forms.ViewModels.Team
{
    public class InviteTeamPageViewModel : BindableBase, IDestructible
    {
        private readonly IUiNavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;
        private Timer _timer;

        public InviteTeamPageViewModel(IUiNavigationService navigationService, IEventAggregator eventAggregator, BrainstormingContext context)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _context = context;
            TeamId = _context.CurrentBrainstormingTeam.Id;
            InitiateMemberCountTimer();
        }

        private void InitiateMemberCountTimer()
        {
            _timer = new Timer(5000);
            _timer.Elapsed += UpdateMemberCount;
            _timer.Start();
        }

        private async void UpdateMemberCount(object sender, ElapsedEventArgs e)
        {
            var newestTeam = new TeamRestResolver().GetTeamById(
                    _context.CurrentBrainstormingTeam.Id);
            _memberCount = newestTeam.CurrentNrOfParticipants;
            if (newestTeam.CurrentNrOfParticipants == _teamCapacity)
            {
                _context.CurrentBrainstormingTeam = newestTeam;
                //_eventAggregator.GetEvent<RenderBrainstormingListEvent>().Publish();
                await _navigationService.NavigateToBrainstormingListTab();
            }
        }

        public void Destroy()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private string _teamId;
        public string TeamId
        {
            get => _teamId;
            set
            {
                SetProperty(ref _teamId, value);
            }
        }

        private string _memberCountString;
        public string MemberCountString
        {
            get => _memberCountString;
            set
            {
                SetProperty(ref _memberCountString, value);
            }
        }

        private int _backendNrCount;
        private int _memberCount
        {
            get => _backendNrCount;
            set
            {
                SetProperty(ref _backendNrCount, value);
                MemberCountString = $"{_memberCount} of {_teamCapacity} Members joined";
            }
        }
        private int _teamCapacity => _context.CurrentBrainstormingTeam.NrOfParticipants;
    }
}
