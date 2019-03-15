using Method635.App.Forms.PrismEvents;
using Prism.Events;
using Prism.Navigation;

using System;
using System.Threading.Tasks;

namespace Method635.App.Forms.Services
{
    public class UiNavigationService : IUiNavigationService
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public UiNavigationService(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            const string BrainstormingFindingListPath = "app:///MainPage?selectedTab=BrainstormingFindingListPage";
            Subscribe<RenderBrainstormingListEvent>(BrainstormingFindingListPath);

            const string RenderBrainstormingPath = "app:///MainPage?selectedTab=BrainstormingPage";
            Subscribe<RenderBrainstormingEvent>(RenderBrainstormingPath);

            const string RenderStartBrainstormingPath = "NavigationPage/StartBrainstormingPage";
            Subscribe<RenderStartBrainstormingEvent>(RenderStartBrainstormingPath);
        }

        private void Subscribe<T>(string navigationPath) where T : PubSubEvent, new()
        {
            Action navigateAsync = async () =>
            {
                await NavigateAsync(navigationPath);
            };

            _eventAggregator.GetEvent<T>().Unsubscribe(navigateAsync);
            _eventAggregator.GetEvent<T>().Subscribe(navigateAsync, ThreadOption.UIThread);
        }

        private async Task NavigateAsync(string path)
        {
            await _navigationService.NavigateAsync(path);
        }

        public async Task NavigateToMainPage()
        {
            //await _navigationService.GoBackToRootAsync();
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

        public Task NavigateToTeamsTab()
        {
            throw new NotImplementedException();
        }

        public async Task NavigateToBrainstormingListTab()
        {
            await _navigationService.NavigateAsync("MainPage?selectedTab=BrainstormingFindingListPage");
        }

        public async Task NavigateToBrainstormingTab()
        {
            await _navigationService.NavigateAsync("app:///MainPage?selectedTab=BrainstormingPage");
        }

        public Task NavigateToCreateBrainstorming()
        {
            throw new NotImplementedException();
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
            await _navigationService.NavigateAsync("NavigationPage/StartBrainstormingPage");
        }
    }
}
