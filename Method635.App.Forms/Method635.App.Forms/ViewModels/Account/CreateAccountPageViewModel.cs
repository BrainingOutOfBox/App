using Method635.App.Models;
using Prism.Commands;
using Prism.Mvvm;
using Method635.App.Logging;
using Method635.App.Forms.Services;
using Method635.App.Forms.Resources;
using Method635.App.BL.Interfaces;
using System.Threading.Tasks;

namespace Method635.App.Forms.ViewModels.Account
{
    public class CreateAccountPageViewModel : BindableBase
    {
        private readonly IUiNavigationService _navigationService;
        private readonly IParticipantService _participantService;

        private readonly ILogger _logger;

        public DelegateCommand RegisterCommand { get; }

        public CreateAccountPageViewModel(ILogger logger, IUiNavigationService navigationService, IParticipantService participantService)
        {
            _logger = logger;
            _navigationService = navigationService;
            _participantService = participantService;
            RegisterCommand = new DelegateCommand(async ()=> await Register());
        }

        private async Task Register()
        {
            if (!CheckInput())
            {
                _logger.Error("Input validation failed");
                return;
            }

            var newParticipant = new Participant()
            {
                FirstName = FirstName,
                LastName = LastName,
                UserName = UserName,
                Password = Password
            };
            if(!_participantService.Register(newParticipant))
            {
                ErrorMessage = AppResources.ErrorRegisteringUser; 
                RegisterFailed = true;
                return;
            }
            await _navigationService.NavigateToLogin();
        }

        private bool CheckInput()
        {
            ErrorMessage = string.Empty;
            RegisterFailed = false;
            if (_password != _repeatPassword)
            {
                ErrorMessage = AppResources.PasswordsDontMatch;
                RegisterFailed = true;
                return false;
            }
            return true;
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set => SetProperty(ref _repeatPassword, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        private bool _registerFailed;
        public bool RegisterFailed
        {
            get => _registerFailed;
            set => SetProperty(ref _registerFailed, value);
        }
    }
}
