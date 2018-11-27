using Method635.App.Forms.Context;
using Method635.App.Forms.Models;
using Method635.App.Forms.RestAccess;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingPageViewModel : BindableBase, INavigatedAware, IActiveAware, IDestructible
    {
        private Timer _timer;
        private readonly INavigationService _navigationService;
        private readonly BrainstormingContext _context;
        private readonly BrainstormingFindingRestResolver _brainstormingFindingRestResolver;

        public DelegateCommand CommitCommand { get; }
        public DelegateCommand SendBrainwaveCommand { get; }

        public BrainstormingPageViewModel(INavigationService navigationService, BrainstormingContext brainstormingContext)
        {
            this._navigationService = navigationService;
            this._context = brainstormingContext;

            this._findingTitle = _context.CurrentFinding?.Name;
            EvaluateDisplayingIdeas();
            this._brainstormingFindingRestResolver = new BrainstormingFindingRestResolver();

            this.CommitCommand = new DelegateCommand(CommitIdea);
            this.SendBrainwaveCommand = new DelegateCommand(SendBrainWave);
            TimerSetup();
        }
        
        private void SendBrainWave()
        {
            var nrOfBrainsheets = _context.CurrentFinding.BrainSheets.Count;
            var currentSheet = _context.CurrentFinding.BrainSheets[(_context.CurrentFinding.CurrentRound + _positionInTeam - 1) % nrOfBrainsheets];
            if(!_brainstormingFindingRestResolver.UpdateSheet(_context.CurrentFinding.Id, currentSheet))
            {
                Console.WriteLine("Couldn't place brainsheet");
            }
            _context.CurrentFinding = _brainstormingFindingRestResolver.GetFinding(_context.CurrentFinding);
            commitIdeaIndex = 0;
            EvaluateDisplayingIdeas();
        }

        private void CommitIdea()
        {
            this.BrainWaves[_context.CurrentFinding.CurrentRound - 1].Ideas[commitIdeaIndex%(_context.CurrentFinding.NrOfIdeas)].Description = IdeaText;
            commitIdeaIndex++;
            IdeaText = string.Empty;
        }

        private void EvaluateDisplayingIdeas()
        {
            if(_context.CurrentBrainstormingTeam==null || _context.CurrentFinding == null || !IsBrainstormingRunning())
            {
                return;
            }
            var currentRound = _context.CurrentFinding.CurrentRound;
            var nrOfBrainsheets = _context.CurrentFinding.BrainSheets.Count;
            //this.IdeaList = _context.CurrentFinding.BrainSheets[(currentRound + positionInTeam - 1) % nrOfBrainsheets].BrainWaves[currentRound-1].Ideas;

            BrainWaves = _context.CurrentFinding.BrainSheets[(currentRound + _positionInTeam - 1) % nrOfBrainsheets].BrainWaves;

        }
        private int _positionInTeam => _teamParticipants.IndexOf(_teamParticipants.Find(p => p.UserName.Equals(_context.CurrentParticipant.UserName)));
        private List<Participant> _teamParticipants  => _context.CurrentBrainstormingTeam.Participants;

        private bool IsBrainstormingRunning()
        {
            return _context.CurrentFinding?.CurrentRound > 0;
        }
        private bool HasBrainstormingEnded()
        {
            return _context.CurrentFinding?.CurrentRound == -1;
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
            _context.CurrentFinding = this._brainstormingFindingRestResolver.GetFinding(_context.CurrentFinding);
            if (IsBrainstormingRunning() || HasBrainstormingEnded())
            {
                // Brainstorming has already started
                CurrentSheetText = $"Sheet {_context.CurrentFinding.CurrentRound} of {_context.CurrentBrainstormingTeam.NrOfParticipants}";
                EvaluateDisplayingIdeas();
                return;
            }

            var moderatorOfCurrentFinding = GetModeratorOfTeam(_context.CurrentFinding.TeamId);
            if (this._context.CurrentParticipant.UserName.Equals(moderatorOfCurrentFinding.UserName))
            {
                // Brainstorming is not yet started and current user is the moderator -> Display StartBrainstorming
                this._navigationService.NavigateAsync("StartBrainstormingPage");
            }
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

        private List<BrainWave> _brainWaves;
        public List<BrainWave> BrainWaves
        {
            get => _brainWaves;
            private set => SetProperty(ref _brainWaves, value);
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

        public bool CommitEnabled => BrainWaves != null;


        private string _ideaText;
        public string IdeaText
        {
            get => _ideaText;
            set => SetProperty(ref _ideaText, value);
        }

        public string Title => "Brainstorming";
        private string _findingTitle;

        public event EventHandler IsActiveChanged;

        public string FindingTitle
        {
            get => _findingTitle;
            set
            {
                SetProperty(ref _findingTitle, value);
            }
        }
        private string _currentSheetText;
        public string CurrentSheetText
        {
            get => _currentSheetText;
            set => SetProperty(ref _currentSheetText, value);
        }

        private bool _isActive;
        private int commitIdeaIndex = 0;

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
