using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace Method635.App.Forms.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            SubscribeToEvents();

            NavigateCommand = new DelegateCommand<string>(OnNavigateCommandExecuted);
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

        private async void OnNavigateCommandExecuted(string path)
        {
            await _navigationService.NavigateAsync(path); 
        }

        public DelegateCommand<string> NavigateCommand { get; }
    }
}
