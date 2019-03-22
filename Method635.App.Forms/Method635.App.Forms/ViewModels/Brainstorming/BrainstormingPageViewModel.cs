using Method635.App.Forms.Context;
using Method635.App.Models;
using Method635.App.Forms.RestAccess;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Forms.Resources;
using Method635.App.Forms.Services;
using Method635.App.Dal.Config;
using Method635.App.BL;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingPageViewModel : BindableBase, INavigatedAware, IActiveAware, IDestructible
    {
        private Timer _updateRoundTimer;
        private Timer _nextCheckRoundTimer;
        private readonly IUiNavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IConfigurationService _configurationService;
        private readonly BrainstormingContext _context;
        private readonly BrainstormingFindingRestResolver _brainstormingFindingRestResolver;
        private int commitIdeaIndex = 0;


        // Platform independent logger necessary, thus resolving from xf dependency service.
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public DelegateCommand CommitCommand { get; }
        public DelegateCommand SendBrainwaveCommand { get; }
        public DelegateCommand RefreshCommand { get; }

        public BrainstormingPageViewModel(
            IUiNavigationService navigationService, 
            IEventAggregator eventAggregator,
            IConfigurationService configurationService, 
            BrainstormingContext brainstormingContext,
            BrainstormingService brainstormingService)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _configurationService = configurationService;
            _context = brainstormingContext;
            _findingTitle = _context.CurrentFinding?.Name;
            _brainstormingFindingRestResolver = new BrainstormingFindingRestResolver();

            CommitCommand = new DelegateCommand(CommitIdea);
            SendBrainwaveCommand = new DelegateCommand(SendBrainWave);
            RefreshCommand = new DelegateCommand(RefreshPage);
            EvaluateDisplayedBrainWaves();
            RemainingTimeTimerSetup();
            BrainWaveSent = false;
        }

        private void RefreshPage()
        {
            _navigationService.NavigateToBrainstormingTab();
        }

        private void SendBrainWave()
        {
            try
            {
                var nrOfBrainsheets = _context.CurrentFinding.BrainSheets.Count;
                var currentSheet = _context.CurrentFinding.BrainSheets[(_context.CurrentFinding.CurrentRound + _positionInTeam - 1) % nrOfBrainsheets];
                if (!_brainstormingFindingRestResolver.UpdateSheet(_context.CurrentFinding.Id, currentSheet))
                {
                    _logger.Error("Couldn't place brainsheet");
                }
                BrainWaveSent = true;
                RoundStartedTimerSetup();

            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Error("Invalid index access!");
            }
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
            var backendFinding = _brainstormingFindingRestResolver.GetFinding(_context.CurrentFinding.Id);
            if (backendFinding?.CurrentRound != _context.CurrentFinding.CurrentRound)
            {
                _context.CurrentFinding = backendFinding;
                _logger.Info("Round has changed, proceeding to next round");
                NextRound();
            }
        }

        private void NextRound()
        {
            _nextCheckRoundTimer.Stop();
            _nextCheckRoundTimer.Dispose();
            BrainWaveSent = false;
            commitIdeaIndex = 0;
            EvaluateDisplayedBrainWaves();
        }

        private void CommitIdea()
        {
            try
            {
                BrainWaves[_context.CurrentFinding.CurrentRound - 1].Ideas[commitIdeaIndex % (_context.CurrentFinding.NrOfIdeas)].Description = IdeaText;
                commitIdeaIndex++;
                IdeaText = string.Empty;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Error("Invalid index access!");
            }
        }

        private void EvaluateDisplayedBrainWaves()
        {
            if (_context.CurrentBrainstormingTeam == null || _context.CurrentFinding == null)
            {
                return;
            }
            if (IsWaiting())
            {
                RoundStartedTimerSetup();
                return;
            }
            BrainSheets = new ObservableCollection<BrainSheet>(_context.CurrentFinding?.BrainSheets);
            if (HasBrainstormingEnded())
            {
                IsBrainstormingFinished = true;
                BrainWaves = _context.CurrentFinding.BrainSheets[CurrentSheetNr++% BrainSheets.Count].BrainWaves;
                return;
            }

            var currentRound = _context.CurrentFinding.CurrentRound;

            var nrOfBrainsheets = BrainSheets.Count;
            CurrentSheetNr = (currentRound + _positionInTeam - 1) % nrOfBrainsheets;
            var currentBrainSheet = _context.CurrentFinding.BrainSheets[CurrentSheetNr];
            BrainWaves = currentBrainSheet.BrainWaves;
            CommitEnabled = BrainWaves != null || _context.CurrentFinding?.CurrentRound > 0;
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
            _updateRoundTimer = new Timer(1000);
            _updateRoundTimer.Elapsed += UpdateRoundTime;
        }

        private void UpdateRoundTime(object sender, ElapsedEventArgs e)
        {
            var remainingTime = _brainstormingFindingRestResolver.GetRemainingTime(
                _context.CurrentFinding.Id,
                _context.CurrentFinding.TeamId);

            if (IsBrainstormingRunning() && remainingTime < TimeSpan.FromSeconds(1) && !BrainWaveSent)
            {
                SendBrainWave();
                return;
            }
            RemainingTime = $"{remainingTime.Minutes:D2}m:{remainingTime.Seconds:D2}s";
        }


        // Navigation away from current page
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _updateRoundTimer.Stop();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var retrievedFinding = _brainstormingFindingRestResolver.GetFinding(_context.CurrentFinding.Id);
            if (retrievedFinding == null)
            {
                _logger.Error($"Finding retrieved from backend was null ({_context.CurrentFinding.Id})");
            }
            _context.CurrentFinding = retrievedFinding;
            if (IsBrainstormingRunning() || HasBrainstormingEnded())
            {
                // Brainstorming has already started
                CurrentSheetText = string.Format(AppResources.SheetNrOfNr, CurrentSheetNr, _context.CurrentBrainstormingTeam.NrOfParticipants);
                EvaluateDisplayedBrainWaves();
                return;
            }

            var moderatorOfCurrentFinding = GetModeratorOfTeam(_context.CurrentFinding.TeamId);
            if (_context.CurrentParticipant.UserName.Equals(moderatorOfCurrentFinding.UserName))
            {
                // Brainstorming is not yet started and current user is the moderator -> Display StartBrainstorming
                _navigationService.NavigateToStartBrainstorming();
                //_eventAggregator.GetEvent<RenderStartBrainstormingEvent>().Publish();
            }
        }

        private Moderator GetModeratorOfTeam(string teamId)
        {
            return new TeamRestResolver().GetModeratorByTeamId(teamId);
        }

        public void Destroy()
        {
           _updateRoundTimer.Stop();
           _updateRoundTimer.Dispose();
        }


        private ObservableCollection<BrainSheet> _brainSheets;
        public ObservableCollection<BrainSheet> BrainSheets
        {
            get => _brainSheets;
            private set
            {
                SetProperty(ref _brainSheets, value);
                _logger.Info($"Set brainsheet with count {value.Count}. Current Sheet nr: {CurrentSheetNr}");

            }
        }

        private List<BrainWave> _brainWaves;
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

        private bool _commitEnabled;
        public bool CommitEnabled
        {
            get => _commitEnabled;
            set => SetProperty(ref _commitEnabled, value); 
        }

        private string _ideaText;
        public string IdeaText
        {
            get => _ideaText;
            set => SetProperty(ref _ideaText, value);
        }

        public string Title => AppResources.Brainstorming;

        public event EventHandler IsActiveChanged;

        private string _findingTitle;
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

        private bool _isBrainstormingFinished;
        public bool IsBrainstormingFinished
        {
            get => _isBrainstormingFinished;
            set => SetProperty(ref _isBrainstormingFinished, value);
        }

        private bool _brainWaveSent;
        public bool BrainWaveSent
        {
            get => _brainWaveSent;
            set => SetProperty(ref _brainWaveSent, value);
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value);

                if (_isActive && !_updateRoundTimer.Enabled && !IsBrainstormingFinished)
                {
                    _updateRoundTimer.Start();
                }
                else if (!_isActive)
                {
                    _updateRoundTimer.Stop();
                }
            }
        }
    }
}
