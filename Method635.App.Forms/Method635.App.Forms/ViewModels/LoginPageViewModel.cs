using Method635.App.Forms.Resources;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Forms.ViewModels
{
    public class LoginPageViewModel
    {
        private readonly INavigationService _navigationService;

        public LoginPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.LoginCommand = new DelegateCommand(Login);
            this.ShowRegisterCommand = new DelegateCommand(ShowRegister);
        }

        private void ShowRegister()
        {
            this._navigationService.NavigateAsync("NavigationPage/CreateAccountPage");
        }

        private void Login()
        {
            //new RestAccess.RestResolver().
        }


        public DelegateCommand LoginCommand { get; }
        public DelegateCommand ShowRegisterCommand { get; }
    }
}
