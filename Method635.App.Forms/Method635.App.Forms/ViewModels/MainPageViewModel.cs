using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading;
using System.Timers;

namespace Method635.App.Forms.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private bool timerStarted = false;
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            GetTimeCommand = new DelegateCommand(GetTime);
            StartTimeCommand = new DelegateCommand(StartTime);
            TapCommand = new DelegateCommand(StartBrainstorming);
        }

        public DelegateCommand StartTimeCommand { get; set; }
        private void StartTime()
        {
            try
            {
                new RestResolver().StartTimer();
            }
            catch(RestEndpointException ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Time started");
        }
        
        public DelegateCommand GetTimeCommand { get; set; }
        private void GetTime()
        {
            if (!timerStarted)
            {
                timerStarted = true;
                var timer = new System.Timers.Timer(1000);
                timer.Elapsed += UpdateRoundTime;
                timer.Start();
            }
            Console.WriteLine("Getting Time..");
        }
        private void UpdateRoundTime(object sender, ElapsedEventArgs e)
        {
            RemainingTime = new RestResolver().GetTime();
        }

        public DelegateCommand TapCommand { get; set; }
        private void StartBrainstorming()
        {
            this._navigationService.NavigateAsync("BrainstormingPage");
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

        private string _startTimeText = "Start Server Timer";
        public string StartTimeText
        {
            get => _startTimeText;
            set
            {
                SetProperty(ref _startTimeText, value);
            }
        }

        private string _getTimeText = "Get Time";
        public string GetTimeText
        {
            get => _getTimeText;
            set
            {
                SetProperty(ref _getTimeText, value);
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
