﻿using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class StartBrainstormingPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;

        public StartBrainstormingPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, BrainstormingContext context)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _context = context;
            TapCommand = new DelegateCommand(StartBrainstorming);
        }

        public DelegateCommand TapCommand { get; set; }
        private void StartBrainstorming()
        {
            new BrainstormingFindingRestResolver().StartBrainstormingFinding(_context.CurrentFinding.Id);
            _eventAggregator.GetEvent<RenderBrainstormingEvent>().Publish();
        }

        private string _connectionErrorText;
        public string ConnectionErrorText
        {
            get => _connectionErrorText;
            set
            {
                SetProperty(ref _connectionErrorText, value);
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
