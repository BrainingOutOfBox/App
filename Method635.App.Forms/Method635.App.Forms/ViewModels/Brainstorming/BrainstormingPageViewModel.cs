using Method635.App.Forms.Context;
using Method635.App.Forms.Models;
using Method635.App.Forms.RestAccess;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingPageViewModel : BindableBase, INavigatedAware, IActiveAware, IDestructible
    {
        private Timer _updateRoundTimer;
        private Timer _nextCheckRoundTimer;
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
            this._brainstormingFindingRestResolver = new BrainstormingFindingRestResolver();

            this.CommitCommand = new DelegateCommand(CommitIdea);
            this.SendBrainwaveCommand = new DelegateCommand(SendBrainWave);

            EvaluateDisplayedBrainWaves();
            RemainingTimeTimerSetup();
        }

        private void SendBrainWave()
        {
            var nrOfBrainsheets = _context.CurrentFinding.BrainSheets.Count;
            var currentSheet = _context.CurrentFinding.BrainSheets[(_context.CurrentFinding.CurrentRound + _positionInTeam - 1) % nrOfBrainsheets];
            if (!_brainstormingFindingRestResolver.UpdateSheet(_context.CurrentFinding.Id, currentSheet))
            {
                Console.WriteLine("Couldn't place brainsheet");
            }
            brainWaveSent = true;
            RoundStartedTimerSetup();
        }

        private void RoundStartedTimerSetup()
        {
            _nextCheckRoundTimer = new Timer(2500);
            _nextCheckRoundTimer.Elapsed += NextCheckRoundTimerElapsed;
            _nextCheckRoundTimer.Start();
        }

        private void NextCheckRoundTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateRound();
        }
        private void UpdateRound()
        {
            var backendFinding = _brainstormingFindingRestResolver.GetFinding(_context.CurrentFinding);
            if (backendFinding.CurrentRound != _context.CurrentFinding.CurrentRound)
            {
                _context.CurrentFinding = backendFinding;
                Console.WriteLine("Round has changed, proceeding to next round");
                NextRound();
            }
        }

        private void NextRound()
        {
            _nextCheckRoundTimer.Stop();
            _nextCheckRoundTimer.Dispose();
            brainWaveSent = false;
            commitIdeaIndex = 0;
            EvaluateDisplayedBrainWaves();
        }

        private void CommitIdea()
        {
            this.BrainWaves[_context.CurrentFinding.CurrentRound - 1].Ideas[commitIdeaIndex % (_context.CurrentFinding.NrOfIdeas)].Description = IdeaText;
            commitIdeaIndex++;
            IdeaText = string.Empty;
        }

        private void EvaluateDisplayedBrainWaves()
        {
            if (_context.CurrentBrainstormingTeam == null || _context.CurrentFinding == null || IsWaiting())
            {
                return;
            }
            BrainSheets = new ObservableCollection<BrainSheet>(_context.CurrentFinding?.BrainSheets);

            if (HasBrainstormingEnded())
            {
                return;
            }


            var currentRound = _context.CurrentFinding.CurrentRound;


            var nrOfBrainsheets = BrainSheets.Count;
            CurrentSheetNr = (currentRound + _positionInTeam - 1) % nrOfBrainsheets;
            var currentBrainSheet = _context.CurrentFinding.BrainSheets[CurrentSheetNr];
            BrainWaves = currentBrainSheet.BrainWaves;
        }
        private int _positionInTeam => _teamParticipants.IndexOf(_teamParticipants.Find(p => p.UserName.Equals(_context.CurrentParticipant.UserName)));
        private List<Participant> _teamParticipants => _context.CurrentBrainstormingTeam.Participants;

        private bool IsBrainstormingRunning()
        {
            return _context.CurrentFinding?.CurrentRound > 0;
        }
        private bool HasBrainstormingEnded()
        {
            return _context.CurrentFinding?.CurrentRound == -1;
        }
        private bool IsWaiting()
        {
            return _context.CurrentFinding?.CurrentRound == 0;
        }

        private void RemainingTimeTimerSetup()
        {
            this._updateRoundTimer = new Timer(1000);
            _updateRoundTimer.Elapsed += UpdateRoundTime;
        }

        private void UpdateRoundTime(object sender, ElapsedEventArgs e)
        {
            var remainingTime = _brainstormingFindingRestResolver.GetRemainingTime(
                _context.CurrentFinding.Id,
                _context.CurrentFinding.TeamId);

            if (IsBrainstormingRunning() && remainingTime < TimeSpan.FromSeconds(1) && !brainWaveSent)
            {
                SendBrainWave();
                return;
            }
            RemainingTime = ($"{remainingTime.Minutes:D2}m:{remainingTime.Seconds:D2}s");
        }


        // Navigation away from current page
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            _updateRoundTimer.Stop();
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            _context.CurrentFinding = this._brainstormingFindingRestResolver.GetFinding(_context.CurrentFinding);
            if (IsBrainstormingRunning() || HasBrainstormingEnded())
            {
                // Brainstorming has already started
                CurrentSheetText = $"Sheet {CurrentSheetNr} of {_context.CurrentBrainstormingTeam.NrOfParticipants}";
                EvaluateDisplayedBrainWaves();
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
            this._updateRoundTimer.Stop();
            this._updateRoundTimer.Dispose();
        }

        private List<BrainWave> _brainWaves;

        private ObservableCollection<BrainSheet> _brainSheets;
        public ObservableCollection<BrainSheet> BrainSheets
        {
            get => _brainSheets;
            private set
            {
                SetProperty(ref _brainSheets, value);
                Console.WriteLine("-------------------");
                Console.WriteLine($"Set brainsheet with count {value.Count}. Current Sheet nr: {CurrentSheetNr}");

            }
        }
        public List<BrainWave> BrainWaves
        {
            get => _brainWaves;
            private set => SetProperty(ref _brainWaves, value);
        }

        private int _currentSheetNr;
        public int CurrentSheetNr
        {
            get => _currentSheetNr;
            set => SetProperty(ref _currentSheetNr, value);
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

        public bool HasBrainstormingNotEnded => !HasBrainstormingEnded();
        public bool IsBrainstormingFinished => HasBrainstormingEnded();

        private bool _isActive;
        private int commitIdeaIndex = 0;
        private bool brainWaveSent;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value, () => System.Diagnostics.Debug.WriteLine($"{Title} IsActive Changed: {value}"));

                if (_isActive && !this._updateRoundTimer.Enabled && HasBrainstormingNotEnded)
                {
                    this._updateRoundTimer.Start();
                }
                else if (!_isActive)
                {
                    this._updateRoundTimer.Stop();
                }
            }
        }
    }
}
