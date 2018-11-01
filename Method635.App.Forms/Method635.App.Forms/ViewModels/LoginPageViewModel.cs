using Method635.App.Forms.Resources;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Method635.App.Forms.ViewModels
{
    public class LoginPageViewModel
    {

        public LoginPageViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
        }


        private void Login()
        {
            //new RestAccess.RestResolver().
        }

        public DelegateCommand LoginCommand { get; set; }

    }
}
