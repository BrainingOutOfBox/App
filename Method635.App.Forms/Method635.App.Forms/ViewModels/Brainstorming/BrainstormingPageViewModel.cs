﻿using Method635.App.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Method635.App.Forms.Resources;
using Method635.App.Forms.Services;
using Method635.App.BL.Interfaces;
using System.ComponentModel;
using Method635.App.BL.Context;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IUiNavigationService _navigationService;
        private readonly BrainstormingContext _context;
        private IBrainstormingService _brainstormingService;
        private readonly IClipboardService _clipboardService;
        private readonly IToastMessageService _toastMessageService;
        private bool _serviceStarted;


        public DelegateCommand CommitCommand { get; }
        public DelegateCommand SendBrainwaveCommand { get; }
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand TapCommand { get; }
        public DelegateCommand InsertSpecialCommand { get; }
        public DelegateCommand<Idea> DownloadImageCommand { get; }
        public DelegateCommand<string> ClickUrlCommand { get; }
        public DelegateCommand ExportCommand { get; }

        public BrainstormingPageViewModel(
            IUiNavigationService navigationService, 
            BrainstormingContext brainstormingContext,
            IBrainstormingService brainstormingService, 
            IToastMessageService toastMessageService,
            IClipboardService clipboardService)
        {
            _navigationService = navigationService;
            _context = brainstormingContext;
            FindingTitle = brainstormingContext.CurrentFinding?.Name;
            FindingDescription = brainstormingContext.CurrentFinding?.ProblemDescription;
            FindingCategory = brainstormingContext.CurrentFinding?.Category;
            _toastMessageService = toastMessageService;
            _brainstormingService = brainstormingService;
            _clipboardService = clipboardService;
            UpdateProperties();

            CommitCommand = new DelegateCommand(async()=>await CommitIdea());
            SendBrainwaveCommand = new DelegateCommand(SendBrainWave);
            RefreshCommand = new DelegateCommand(RefreshPage);
            TapCommand = new DelegateCommand(StartBrainstorming);
            InsertSpecialCommand = new DelegateCommand(InsertSpecial);
            DownloadImageCommand = new DelegateCommand<Idea>(async (si) => await DownloadImage(si));
            ClickUrlCommand = new DelegateCommand<string>(ClickUrl);
            ExportCommand = new DelegateCommand(async()=>await Export());

            CommitEnabled = true;
        }

        private async Task Export()
        {
            var exportContent = await Task.Run(()=>_brainstormingService.GetExport());

            _clipboardService.CopyToClipboard(exportContent);
            _toastMessageService.LongAlert(AppResources.CopiedToClipboardMessage);
        }

        private void ClickUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }

        private async Task DownloadImage(Idea idea)
        {
            await _brainstormingService.SetPictureImageSource(idea);
        }

        private void InsertSpecial()
        {
            _navigationService.NavigateToInsertSpecial();
        }

        private void StartBrainstorming()
        {
            _brainstormingService.StartBrainstorming();
            var parameters = new NavigationParameters
            {
                { "brainstormingService", _brainstormingService }
            };
            _navigationService.NavigateToBrainstormingTab(parameters);
        }

        private void _brainstormingService_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
            CurrentRoundText = string.Format(AppResources.CurrentRoundCounter, _context.CurrentFinding?.CurrentRound, _brainstormingService.BrainSheets?.Count);
            RemainingTime = $"{_brainstormingService.RemainingTime.Minutes:D2}m:{_brainstormingService.RemainingTime.Seconds:D2}s";
            ShowStartBrainstorming = IsWaiting && _brainstormingService.IsModerator.HasValue && _brainstormingService.IsModerator.Value;
            ShowWaitingBrainstorming = IsWaiting && !(_brainstormingService.IsModerator.HasValue && _brainstormingService.IsModerator.Value);
            CurrentSheetIndex = _brainstormingService.CurrentSheetIndex;
            IdeaHeight = IsEnded ? 400 : 200;
        }

        private void RefreshPage()
        {
            var parameters = new NavigationParameters
            {
                { "brainstormingService", _brainstormingService }
            };
            _navigationService.NavigateToBrainstormingTab(parameters);
        }

        private void SendBrainWave()
        {
            _brainstormingService.SendBrainWave();
            _toastMessageService.LongAlert(AppResources.IdeasSent);
        }

        private async Task CommitIdea()
        {
            await _brainstormingService.CommitIdea(new NoteIdea() { Description = IdeaText });
            IdeaText = string.Empty;
            _toastMessageService.LongAlert(AppResources.IdeaCommited);
        }

        
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.TryGetValue("brainstormingService", out IBrainstormingService brainstormingService);
            if (brainstormingService != null)
                _brainstormingService = brainstormingService;

            _brainstormingService.StopBusinessService();
            _brainstormingService.PropertyChanged -= _brainstormingService_PropertyChanged;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            parameters.TryGetValue("brainstormingService", out IBrainstormingService brainstormingService);
            if (brainstormingService != null)
                _brainstormingService = brainstormingService;

            _brainstormingService.PropertyChanged += _brainstormingService_PropertyChanged;

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

        private int _currentSheetIndex;
        public int CurrentSheetIndex
        {
            get => _currentSheetIndex;
            set
            {
                SetProperty(ref _currentSheetIndex, value);
                CurrentSheetText = string.Format(AppResources.SheetNr, _currentSheetIndex+1);
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
        private string _currentRoundText;
        public string CurrentRoundText
        {
            get =>_currentRoundText;
            private set=>SetProperty(ref _currentRoundText, value);
        }

        private bool _showStartBrainstorming;
        public bool ShowStartBrainstorming
        {
            get => _showStartBrainstorming;
            private set=>SetProperty(ref _showStartBrainstorming, value);
        }
        private bool _showWaitingBrainstorming;
        public bool ShowWaitingBrainstorming
        {
            get=>_showWaitingBrainstorming;
            private set=>SetProperty(ref _showWaitingBrainstorming, value);
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
        private string _findingDescription;
        public string FindingDescription
        {
            get => _findingDescription;
            set => SetProperty(ref _findingDescription, value);
        }

        private string _findingCategory;
        public string FindingCategory
        {
            get=> _findingCategory;
            set => SetProperty(ref _findingCategory, value);
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