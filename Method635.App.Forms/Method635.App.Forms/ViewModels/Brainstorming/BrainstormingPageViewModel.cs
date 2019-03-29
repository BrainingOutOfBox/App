using Method635.App.Forms.Context;
using Method635.App.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Method635.App.Forms.Resources;
using Method635.App.Forms.Services;
using Method635.App.Dal.Config;
using Method635.App.BL.Interfaces;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IUiNavigationService _navigationService;
        private readonly IConfigurationService _configurationService;
        private readonly BrainstormingContext _context;
        private readonly IBrainstormingService _brainstormingService;
        private bool _serviceStarted;


        public DelegateCommand CommitCommand { get; }
        public DelegateCommand SendBrainwaveCommand { get; }
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand TapCommand { get; }

        public BrainstormingPageViewModel(
            IUiNavigationService navigationService, 
            IConfigurationService configurationService, 
            BrainstormingContext brainstormingContext,
            IBrainstormingService brainstormingService)
        {
            _navigationService = navigationService;
            _configurationService = configurationService;
            _context = brainstormingContext;
            _findingTitle = _context.CurrentFinding?.Name;

            _brainstormingService = brainstormingService;
            _brainstormingService.PropertyChanged += _brainstormingService_PropertyChanged;
            UpdateProperties();

            CommitCommand = new DelegateCommand(CommitIdea);
            SendBrainwaveCommand = new DelegateCommand(SendBrainWave);
            RefreshCommand = new DelegateCommand(RefreshPage);
            TapCommand = new DelegateCommand(StartBrainstorming);

            CommitEnabled = true;
        }

        private void StartBrainstorming()
        {
            _brainstormingService.StartBrainstorming();
            _navigationService.NavigateToBrainstormingTab();
        }

        private void _brainstormingService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateProperties();
        }

        private void UpdateProperties()
        {
            BrainSheets = _brainstormingService.BrainSheets;
            BrainWaveSent = _brainstormingService.BrainWaveSent;
            IsWaiting = _brainstormingService.IsWaiting;
            IsRunning = _brainstormingService.IsRunning;
            IsEnded = _brainstormingService.IsEnded;
            RemainingTime = $"{_brainstormingService.RemainingTime.Minutes:D2}m:{_brainstormingService.RemainingTime.Seconds:D2}s";
            ShowStartBrainstorming = IsWaiting && _brainstormingService.IsModerator.Value;

            IdeaHeight = IsEnded ? 450 : 300;
        }

        private void RefreshPage()
        {
            _navigationService.NavigateToBrainstormingTab();
        }

        private void SendBrainWave()
        {
            _brainstormingService.BrainWaveSent = true;
            _brainstormingService.SendBrainWave();
        }

        private void CommitIdea()
        {
            _brainstormingService.CommitIdea(IdeaText);
            IdeaText = string.Empty;
        }

        
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //TODO Implement a pause functionality in business service
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!_serviceStarted)
            {
                _brainstormingService.StartBusinessService();
                _serviceStarted = true;
            }
        }

        private ObservableCollection<BrainSheet> _brainSheets;
        public ObservableCollection<BrainSheet> BrainSheets
        {
            get => _brainSheets;
            private set => SetProperty(ref _brainSheets, value);
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

        private bool _showStartBrainstorming;
        public bool ShowStartBrainstorming
        {
            get => _showStartBrainstorming;
            private set=>SetProperty(ref _showStartBrainstorming, value);
        }
        private int _ideaHeight;
        public int IdeaHeight { get => _ideaHeight; private set=>SetProperty(ref _ideaHeight, value); }

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

        private bool _isWaiting;
        public bool IsWaiting { get => _isWaiting; private set => SetProperty(ref _isWaiting, value); }

        private bool _isEnded;
        public bool IsEnded { get => _isEnded; private set => SetProperty(ref _isEnded, value); }


        private bool _isRunning;
        public bool IsRunning { get => _isRunning; private set => SetProperty(ref _isRunning, value); }
               
    }
}
