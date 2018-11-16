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
    public class BrainstormingPageViewModel : BindableBase, INavigatedAware, IActiveAware, IDestructible
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

            TimerSetup();
        }
        
        private void TimerSetup()
        {
            this._timer = new Timer(1000);
            _timer.Elapsed += UpdateRoundTime;
        }
        private void UpdateRoundTime(object sender, ElapsedEventArgs e)
        {
            RemainingTime = _brainstormingFindingRestResolver.GetRemainingTime(
                _context.CurrentFinding.Id,
                _context.CurrentFinding.TeamId);
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
            // TODO: Comment out once Participant/Team-Logic is implemented 
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

        public void Destroy()
        {
            this._timer.Stop();
            this._timer.Dispose();
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
            set
            {
                SetProperty(ref _isActive, value, () => System.Diagnostics.Debug.WriteLine($"{Title} IsActive Changed: {value}"));

                if (_isActive && !this._timer.Enabled)
                {
                    this._timer.Start();
                }
                else if(!_isActive)
                {
                    this._timer.Stop();
                }
            }
        }
    }
}
