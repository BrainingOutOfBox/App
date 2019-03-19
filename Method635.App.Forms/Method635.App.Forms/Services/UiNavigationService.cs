using Prism.Navigation;
using System.Threading.Tasks;

namespace Method635.App.Forms.Services
{
    public class UiNavigationService : IUiNavigationService
    {
        private readonly INavigationService _navigationService;

        public UiNavigationService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task NavigateToMainPage()
        {
            await _navigationService.NavigateAsync("app:///MainPage");
        }

        public async Task NavigateToLogin()
        {
            await _navigationService.NavigateAsync("/NavigationPage/LoginPage");
        }

        public async Task NavigateToRegister()
        {
            await _navigationService.NavigateAsync("CreateAccountPage");
        }

        public async Task NavigateToTeamsTab()
        {
            await _navigationService.NavigateAsync("app:///MainPage?selectedTab=TeamPage");
        }

        public async Task NavigateToBrainstormingListTab()
        {
            await _navigationService.NavigateAsync("app:///MainPage?selectedTab=BrainstormingFindingListPage");
        }

        public async Task NavigateToBrainstormingTab()
        {
            await _navigationService.NavigateAsync("app:///MainPage?selectedTab=BrainstormingPage");
        }

        public async Task NavigateToCreateBrainstorming()
        {
            await _navigationService.NavigateAsync("/NavigationPage/MainPage/NewBrainstormingPage");
        }

        public async Task NavigateToJoinTeam()
        {
            await _navigationService.NavigateAsync("/NavigationPage/MainPage/JoinTeamPage/");
        }

        public async Task NavigateToCreateTeam()
        {
            await _navigationService.NavigateAsync("/NavigationPage/MainPage/NewTeamPage/");
        }

        public async Task NavigateToInviteTeam()
        {
            await _navigationService.NavigateAsync("InviteTeamPage");
        }

        public async Task NavigateToStartBrainstorming()
        {
            await _navigationService.NavigateAsync("/NavigationPage/StartBrainstormingPage/");
        }
    }
}
