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

namespace Method635.App.Forms.ViewModels.Team
{
    public class TeamPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;


        public TeamPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, BrainstormingContext context)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._context = context;

            this.TeamList = FillTeamList();
            if (this.TeamList.Count > 0 && _context.CurrentBrainstormingTeam == null)
            {
                SelectedTeam = this.TeamList[0];
                _context.CurrentBrainstormingTeam = this.SelectedTeam;
            }
            this.SelectTeamCommand = new DelegateCommand(SelectTeam);
            this.CreateTeamCommand = new DelegateCommand(CreateTeam);
            this.JoinTeamCommand = new DelegateCommand(JoinTeam);
            this.LeaveTeamCommand = new DelegateCommand<BrainstormingTeam>(LeaveTeam);
        }

        private void LeaveTeam(BrainstormingTeam team)
        {
            Console.WriteLine("Leaving team...");
        }

        private void JoinTeam()
        {
            this._navigationService.NavigateAsync("JoinTeamPage");
        }

        private void CreateTeam()
        {
            this._navigationService.NavigateAsync("NewTeamPage");
        }

        private void SelectTeam()
        {
            _context.CurrentBrainstormingTeam = _selectedTeam;
            this._eventAggregator.GetEvent<RenderBrainstormingListEvent>().Publish();
        }

        private List<BrainstormingTeam> FillTeamList()
        {
            var teamList = new TeamRestResolver().GetMyBrainstormingTeams(_context.CurrentParticipant.UserName);
            HasTeam = teamList.Count > 0;
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
        public bool HasNoTeam => !HasTeam;

        public string Title => "My Teams";
	}
}
