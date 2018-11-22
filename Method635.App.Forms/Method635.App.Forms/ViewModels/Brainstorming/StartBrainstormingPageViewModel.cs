using Method635.App.Forms.Context;
using Method635.App.Forms.RestAccess;
using Method635.App.Forms.RestAccess.RestExceptions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class StartBrainstormingPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly BrainstormingContext _context;

        public StartBrainstormingPageViewModel(INavigationService navigationService, BrainstormingContext context)
        {
            this._navigationService = navigationService;
            this._context = context;
            TapCommand = new DelegateCommand(StartBrainstorming);
        }

        public DelegateCommand TapCommand { get; set; }
        private async void StartBrainstorming()
        {
            new BrainstormingFindingRestResolver().StartBrainstormingFinding(this._context.CurrentFinding.Id);
            await this._navigationService.NavigateAsync("BrainstormingPage");
        }

        private string _connectionErrorText;
        public string ConnectionErrorText
        {
            get => _connectionErrorText;
            set
            {
                SetProperty(ref _connectionErrorText, value);
            }
        }

        private string _clickOnTextToStartBrainstorming = "Click on the icon to start Brainstorming";
        public string ClickOnTextToStartBrainstorming
        {
            get => _clickOnTextToStartBrainstorming;
            set
            {
                SetProperty(ref _clickOnTextToStartBrainstorming, value);
            }
        }

    }
}
