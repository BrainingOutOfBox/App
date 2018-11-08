using Method635.App.Forms.Context;
using Method635.App.Forms.Models;
using Method635.App.Forms.RestAccess;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Timers;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingPageViewModel : BindableBase, INavigatedAware, IActiveAware
    {
        private Timer _timer;
        private readonly INavigationService _navigationService;
        private readonly BrainstormingContext _context;
        private readonly BrainstormingFindingRestResolver _brainstormingFindingRestResolver;

        public BrainstormingPageViewModel(INavigationService navigationService, BrainstormingContext brainstormingContext)
        {
            this._navigationService = navigationService;
            this._context = brainstormingContext;

            this._findingTitle = _context.CurrentFinding?.Name;

            this._brainstormingFindingRestResolver = new BrainstormingFindingRestResolver();
            this.SwipeRightCommand = new DelegateCommand(SwipeRight);
            this.SwipeLeftCommand = new DelegateCommand(SwipeLeft);

            TimerSetup();

            IsActiveChanged += ActiveChanged;
        }

        private void ActiveChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{Title} IsActive: {IsActive}");
            if (IsActive)
            {
                this._timer.Start();
            }
            else
            {
                this._timer.Stop();
            }
        }

        private void SwipeRight()
        {
            // TODO: Swipe sheet to the right
        }

        private void SwipeLeft()
        {
            // TODO: Swipe sheet to the left
        }
        
        public DelegateCommand SwipeRightCommand { get; }
        public DelegateCommand SwipeLeftCommand { get; }
        
        private void TimerSetup()
        {
            this._timer = new Timer(1000);
            _timer.Elapsed += UpdateRoundTime;
            _timer.Start();
            Console.WriteLine("Getting Time..");
        }
        private void UpdateRoundTime(object sender, ElapsedEventArgs e)
        {
            try
            {
                RemainingTime = _brainstormingFindingRestResolver.GetRemainingTime(
                    _context.CurrentFinding.TeamId,
                    _context.CurrentFinding.Id);
            }
            catch(RestEndpointException ex)
            {
                Console.WriteLine($"Couldn't get updated round time from backend {ex}");
            }
        }

        // Navigation away from current page
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            _timer.Stop();
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (this._context.CurrentFinding.CurrentRound > 0)
            {
                // Brainstorming has already started
                return;
            }
            // TODO: Comment out once backend supports GetTeamById
            //var moderatorOfCurrentFinding = GetModeratorOfTeam(_context.CurrentFinding.TeamId);
            //if (this._context.CurrentParticipant.UserName.Equals(moderatorOfCurrentFinding.UserName))
            //{
            //    // Brainstorming is not yet started and current user is the moderator -> Display StartBrainstorming
            //    this._navigationService.NavigateAsync("StartBrainstormingPage");
            //}
        }

        private Moderator GetModeratorOfTeam(string teamId)
        {
            return new TeamRestResolver().GetModeratorByTeamId(teamId);
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

        public string Title => "Brainstorming";
        private string _findingTitle;

        public event EventHandler IsActiveChanged;

        public string FindingTitle {
            get => _findingTitle;
            set
            {
                SetProperty(ref _findingTitle, value);
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, () => System.Diagnostics.Debug.WriteLine($"{Title} IsActive Changed: {value}")); }
        }
    }
}
