using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Resources;
using Method635.App.Forms.RestAccess;
using Method635.App.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class StartBrainstormingPageViewModel : BindableBase
	{
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;
        private readonly ILogger _logger;

        public StartBrainstormingPageViewModel(IEventAggregator eventAggregator, ILogger logger, BrainstormingContext context)
        {
            _eventAggregator = eventAggregator;
            _context = context;
            _logger = logger;
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
