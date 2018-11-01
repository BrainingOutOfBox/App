using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Method635.App.Forms.ViewModels
{
	public class BrainstormingPageViewModel : BindableBase
    {
        private bool _timerStarted;
        private readonly INavigationService _navigationService;
        private readonly BrainstormingFindingRestResolver _brainstormingFindingRestResolver;

        public BrainstormingPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this._brainstormingFindingRestResolver = new BrainstormingFindingRestResolver();
            this.OpenNavigationMenuCommand = new DelegateCommand(OpenNavigationMenu);
            GetTime();
        }
        
        public DelegateCommand OpenNavigationMenuCommand { get; }
        private void OpenNavigationMenu()
        {
            Console.WriteLine("Opening navigation menu...");
        }

        private void GetTime()
        {
            if (!_timerStarted)
            {
                _timerStarted = true;
                var timer = new Timer(1000);
                timer.Elapsed += UpdateRoundTime;
                timer.Start();
            }
            Console.WriteLine("Getting Time..");
        }
        private void UpdateRoundTime(object sender, ElapsedEventArgs e)
        {
            try
            {
                RemainingTime = _brainstormingFindingRestResolver.GetRemainingTime();
            }
            catch(RestEndpointException ex)
            {
                Console.WriteLine("Couldn't get updated round time from backend", ex);
            }
        }


        private string _remainingTime;
        public string RemainingTime
        {
            get => _remainingTime;
            set
            {
                SetProperty(ref _remainingTime, value);
            }
        }
    }
}
