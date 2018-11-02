using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class StartBrainstormingPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        public StartBrainstormingPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            TapCommand = new DelegateCommand(StartBrainstorming);
        }

        public DelegateCommand TapCommand { get; set; }
        private async void StartBrainstorming()
        {
            try
            {
                new BrainstormingFindingRestResolver().CreateBrainstormingFinding(finding: null);
                await this._navigationService.NavigateAsync("BrainstormingPage");
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Error starting brainstorming: {ex}");
                ConnectionErrorText = "There was an error connecting to the server..";
            }
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
