﻿using Method635.App.BL.Context;
using Method635.App.BL.Interfaces;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Resources;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Timers;

namespace Method635.App.Forms.ViewModels.Team
{
    public class InviteTeamPageViewModel : BindableBase, IDestructible
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ITeamService _teamService;
        private readonly BrainstormingContext _context;
        private Timer _timer;
        private int _teamCapacity => _context.CurrentBrainstormingTeam.NrOfParticipants;

        public InviteTeamPageViewModel(
            IEventAggregator eventAggregator,
            ITeamService teamService,
            BrainstormingContext context)
        {
            _eventAggregator = eventAggregator;
            _teamService = teamService;
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
            var newestTeam = await Task.Run(() => _teamService.GetCurrentTeam());
            _memberCount = newestTeam.CurrentNrOfParticipants;
            if (newestTeam.CurrentNrOfParticipants == _teamCapacity)
            {
                _context.CurrentBrainstormingTeam = newestTeam;
                TeamFull = true;
                _eventAggregator.GetEvent<InviteTeamCompleteEvent>().Publish();
            }
        }

        public void Destroy()
        {
            _timer.Stop();
            _timer.Elapsed -= UpdateMemberCount;
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
                MemberCountString = string.Format(AppResources.MemberCountString, _memberCount,_teamCapacity);
            }
        }

        private bool _teamFull;
        public bool TeamFull { get=>_teamFull; private set=>SetProperty(ref _teamFull, value)   ; }
    }
}
