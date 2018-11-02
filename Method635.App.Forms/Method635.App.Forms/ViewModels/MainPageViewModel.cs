using System;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Method635.App.Forms.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            SubscribeToEvents();

            this.NavigateCommand = new DelegateCommand<string>(OnNavigateCommandExecuted);
        }

        private void SubscribeToEvents()
        {
            this._eventAggregator.GetEvent<RenderBrainstormingEvent>().Subscribe(() =>
            {
                this._navigationService.NavigateAsync("BrainstormingPage");
            }, ThreadOption.UIThread);
        }

        private async void OnNavigateCommandExecuted(string path)
        {
            await this._navigationService.NavigateAsync(path);
        }

        public DelegateCommand<string> NavigateCommand { get; }

    }
}
