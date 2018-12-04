using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System.Timers;
using ZXing.Common;

namespace Method635.App.Forms.ViewModels.Team
{
    public class InviteTeamPageViewModel : BindableBase, IDestructible
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;

        public InviteTeamPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, BrainstormingContext context)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._context = context;
            TeamId = _context.CurrentBrainstormingTeam.Id;
            InitiateMemberCountTimer();
        }

        private void InitiateMemberCountTimer()
        {
            this._timer = new Timer(5000);
            _timer.Elapsed += UpdateMemberCount;
            _timer.Start();
        }

        private void UpdateMemberCount(object sender, ElapsedEventArgs e)
        {
            var newestTeam = new TeamRestResolver().GetTeamById(
                    _context.CurrentBrainstormingTeam.Id);
            _memberCount = newestTeam.CurrentNrOfParticipants;
            if (newestTeam.CurrentNrOfParticipants == _teamCapacity)
            {
                _context.CurrentBrainstormingTeam = newestTeam;
                this._eventAggregator.GetEvent<RenderBrainstormingListEvent>().Publish();
            }
        }

        public void Destroy()
        {
            this._timer.Stop();
            this._timer.Dispose();
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
        private Timer _timer;

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
