using System;
using Method635.App.Forms.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Method635.App.Forms.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.NavigateCommand = new DelegateCommand<string>(OnNavigateCommandExecuted);
        }

        private async void OnNavigateCommandExecuted(string path)
        {
            await this._navigationService.NavigateAsync(path);
        }

        public DelegateCommand<string> NavigateCommand { get; }

        private ContentPage _masterPage = new MasterPage();
        public ContentPage MasterPage { get; set; }

    }
}
