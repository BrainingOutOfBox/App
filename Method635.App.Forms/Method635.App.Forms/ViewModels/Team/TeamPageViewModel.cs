using Method635.App.Forms.Context;
using Method635.App.Forms.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace Method635.App.Forms.ViewModels.Team
{
    public class TeamPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly BrainstormingContext _context;


        public TeamPageViewModel(INavigationService navigationService, BrainstormingContext context)
        {
            this._navigationService = navigationService;
            this._context = context;

            this.TeamList = FillTeamList();

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
            _context.CurrentBrainstormingTeam = SelectedTeam;
        }

        private List<BrainstormingTeam> FillTeamList()
        {
            return new List<BrainstormingTeam>(){
                new BrainstormingTeam(){
                    Name="A-Team" } };
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
        public string Title => "My Teams";
	}
}
