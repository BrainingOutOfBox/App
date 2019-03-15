using Method635.App.Forms.Context;
using Method635.App.Models;
using Method635.App.Forms.Resources;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Forms.Services;

namespace Method635.App.Forms.ViewModels
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly IUiNavigationService _navigationService;
        private readonly BrainstormingContext _context;

        // Platform independent logger necessary, thus resolving from xf dependency service.
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public LoginPageViewModel(IUiNavigationService navigationService, BrainstormingContext context)
        {
            _navigationService = navigationService;
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
            var response = new ParticipantRestResolver().Login(loginParticipant);
            if(response != null)
            {
                _context.JwtToken = response.JwtToken;
                _context.CurrentParticipant = response.Participant;
                await _navigationService.NavigateToMainPage();
                return;
            }
            ErrorText = "There was an error with your login, please try again.";
            HasError = true;
        }

        private bool CheckInput()
        {
            ErrorText = string.Empty;
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(UserName))
            {
                ErrorText = "Please fill in all the fields to login";
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
