using Method635.App.Forms.PrismEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;

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
            _eventAggregator.GetEvent<RenderBrainstormingListEvent>().Subscribe(async () =>
            {
                await _navigationService.NavigateAsync("app:///MainPage?selectedTab=BrainstormingFindingListPage");
            }, ThreadOption.UIThread);

            _eventAggregator.GetEvent<RenderBrainstormingEvent>().Subscribe(async () =>
            {
                await _navigationService.NavigateAsync("app:///MainPage?selectedTab=BrainstormingPage");
            }, ThreadOption.UIThread);
        }

        private async void OnNavigateCommandExecuted(string path)
        {
            await _navigationService.NavigateAsync(path);
        }

        public DelegateCommand<string> NavigateCommand { get; }
    }
}
