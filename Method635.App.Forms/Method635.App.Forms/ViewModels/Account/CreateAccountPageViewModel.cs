﻿using System;
using Method635.App.Forms.Models;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace Method635.App.Forms.ViewModels.Account
{
    public class CreateAccountPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public DelegateCommand RegisterCommand { get; }

        public CreateAccountPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.RegisterCommand = new DelegateCommand(Register);
        }

        private async void Register()
        {
            if (!CheckInput())
            {
                Console.WriteLine("Input validation failed");
                return;
            }

            var newParticipant = new Participant()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                UserName = this.UserName,
                Password = this.Password
            };
            if(!new ParticipantRestResolver().CreateParticipant(newParticipant))
            {
                ErrorMessage = "There was an error registering your user.";
                RegisterFailed = true;
                return;
            }
            await _navigationService.NavigateAsync("app:///NavigationPage/LoginPage");
        }

        private bool CheckInput()
        {
            ErrorMessage = string.Empty;
            RegisterFailed = false;
            if (_password != _repeatPassword)
            {
                ErrorMessage = "Please make sure the passwords match.";
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