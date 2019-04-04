using Method635.App.Models;
using Method635.App.Forms.Resources;
using Prism.Commands;
using Prism.Mvvm;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Forms.Services;
using Method635.App.Dal.Interfaces;
using Method635.App.BL.Context;

namespace Method635.App.Forms.ViewModels
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly IUiNavigationService _navigationService;
        private readonly IParticipantDalService _participantDalService;
        private readonly BrainstormingContext _context;

        // Platform independent logger necessary, thus resolving from xf dependency service.
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public LoginPageViewModel(IUiNavigationService navigationService,
            IDalService dalService,
            BrainstormingContext context)
        {
            _navigationService = navigationService;
            _participantDalService = dalService.ParticipantDalService;
            _context = context;

            LoginCommand = new DelegateCommand(Login);
            ShowRegisterCommand = new DelegateCommand(ShowRegister);
        }

        private void ShowRegister()
        {
            _navigationService.NavigateToRegister();
        }

        private async void Login()
        {
            if (!CheckInput())
            {
                _logger.Error("Input invalid for login");
                return;
            }
            var loginParticipant = new Participant()
            {
                UserName = UserName,
                Password = Password
            };
            var participant = _participantDalService.Login(loginParticipant);
            if(participant != null)
            {
                _context.CurrentParticipant = participant;
                await _navigationService.NavigateToMainPage();
                return;
            }
            ErrorText = AppResources.LoginError;
            HasError = true;
        }

        private bool CheckInput()
        {
            ErrorText = string.Empty;
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(UserName))
            {
                ErrorText = AppResources.FillNecessaryFields;
                HasError = true;
                return false;
            }
            return true;
        }

        private string _username;
        public string UserName
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }
        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }
        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand ShowRegisterCommand { get; }
    }
}
