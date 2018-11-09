using Method635.App.Forms.Context;
using Method635.App.Forms.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;

namespace Method635.App.Forms.ViewModels.Team
{
    public class BrainstormingTeamPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly BrainstormingContext _context;


        public BrainstormingTeamPageViewModel(INavigationService navigationService, BrainstormingContext context)
        {
            this._navigationService = navigationService;
            this._context = context;

            this.TeamList = FillTeamList();

            this.SelectTeamCommand = new DelegateCommand(SelectTeam);
            this.CreateTeamCommand = new DelegateCommand(CreateTeam);
            this.JoinTeamCommand = new DelegateCommand(JoinTeam);
        }

        private void JoinTeam()
        {
            //TODO Call JoinTeam Page
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
        public BrainstormingTeam SelectedTeam { get; set; }
        public string Title => "My Teams";
	}
}
