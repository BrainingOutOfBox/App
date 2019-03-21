using Method635.App.Forms.Context;
using Method635.App.Forms.Resources;
using Method635.App.Forms.RestAccess;
using Method635.App.Forms.Services;
using Method635.App.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class StartBrainstormingPageViewModel : BindableBase
	{
        private readonly IUiNavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;
        private readonly ILogger _logger;

        public StartBrainstormingPageViewModel(IUiNavigationService navigationService, IEventAggregator eventAggregator, ILogger logger, BrainstormingContext context)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _context = context;
            _logger = logger;
            TapCommand = new DelegateCommand(StartBrainstorming);
        }

        public DelegateCommand TapCommand { get; set; }
        private void StartBrainstorming()
        {
            new BrainstormingFindingRestResolver().StartBrainstormingFinding(_context.CurrentFinding.Id);
            _navigationService.NavigateToBrainstormingTab();
            //_eventAggregator.GetEvent<RenderBrainstormingEvent>().Publish();
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

        private string _clickOnTextToStartBrainstorming = AppResources.ClickToStartBrainstorming;
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
