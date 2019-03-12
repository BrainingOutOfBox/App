using Method635.App.Forms.Context;
using Method635.App.Forms.Models;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Method635.App.Forms.ViewModels.Team
{
    public class TeamPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;

        public TeamPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, BrainstormingContext context)
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
            Console.WriteLine("Leaving team...");
        }

        private void JoinTeam()
        {
            _navigationService.NavigateAsync("JoinTeamPage");
        }

        private void CreateTeam()
        {
            _navigationService.NavigateAsync("NewTeamPage");
        }

        private void SelectTeam()
        {
            _context.CurrentBrainstormingTeam = _selectedTeam;
            _eventAggregator.GetEvent<RenderBrainstormingListEvent>().Publish();
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

        public string Title => "My Teams";
	}
}
