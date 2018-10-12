using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading;
using System.Timers;

namespace Method635.App.Forms.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public MainPageViewModel()
        {
            GetTimeCommand = new DelegateCommand(GetTime);
            StartTimeCommand = new DelegateCommand(StartTime);
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
        private bool timerStarted = false;
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
    }
}
