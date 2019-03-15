using Method635.App.Forms.Context;
using Method635.App.Models;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using Method635.App.Logging;
using Xamarin.Forms;
using Method635.App.Forms.Resources;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class NewBrainstormingPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context; 
        private int _nrOfIdeas;
        private int _baseRoundTime;

        private readonly List<char> disallowedChars = new List<char>{ '\\', ' ' };

        // Platform independent logger necessary, thus resolving from xf dependency service.
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public DelegateCommand CreateFindingCommand { get; }

        public NewBrainstormingPageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator,
            BrainstormingContext brainstormingContext)
        {
           _navigationService = navigationService;
           _eventAggregator = eventAggregator;
           _context = brainstormingContext;
           CreateFindingCommand = new DelegateCommand(CreateFinding);

           HasInvalidChars = false;
        }

        private void CreateFinding()
        {
            if (!CheckInput())
            {
                _logger.Error($"Invalid input (using '{FindingName}')to create finding..");
                return;
            }
            try
            {
                var finding = new BrainstormingFinding()
                {
                    TeamId = _context.CurrentBrainstormingTeam.Id,
                    Name = FindingName,
                    NrOfIdeas = _nrOfIdeas,
                    BaseRoundTime = _baseRoundTime,
                    ProblemDescription = Description
                };
                finding = new BrainstormingFindingRestResolver().CreateBrainstormingFinding(finding);
                _context.CurrentFinding = finding;
                _eventAggregator.GetEvent<RenderBrainstormingEvent>().Publish();
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            
        }

        private bool CheckInput()
        {
            if (!HasValidFindingName())
            {
                ErrorText = AppResources.DontUseProhibitedChars;
                HasError = true;
                HasInvalidChars = true;
                return false;
            }
            if(string.IsNullOrEmpty(FindingName) ||
                string.IsNullOrEmpty(NrOfIdeasText) ||
                string.IsNullOrEmpty(BaseRoundTimeText))
            {
                ErrorText = AppResources.FillNecessaryFields;
                HasError = true;
                return false;
            }
            if (!int.TryParse(NrOfIdeasText, out int nrOfIdeas) ||
                !int.TryParse(BaseRoundTimeText, out int baseRoundTime))
            {
                ErrorText = AppResources.UseNumbersInFields;
                HasError = true;
                return false;
            }
            _baseRoundTime = baseRoundTime;
            _nrOfIdeas = nrOfIdeas;
            return true;
        }

        private bool HasValidFindingName()
        {
            HasInvalidChars = false;
            if (string.IsNullOrEmpty(FindingName)) return false;
            return disallowedChars.TrueForAll(c => FindingName.IndexOf(c) < 0);
        }

        public string FindingName { get; set; }
        //TODO Input Validation for ints
        public string NrOfIdeasText { get; set; }
        public string BaseRoundTimeText { get; set; }
        public string Description { get; set; } = string.Empty;

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }
        public string ProhibitedChars => string.Format(AppResources.ProhibitedCharsText,string.Join(", ", disallowedChars));

        private bool _hasInvalidChars;
        public bool HasInvalidChars
        {
            get => _hasInvalidChars;
            set => SetProperty(ref _hasInvalidChars, value);
        }
    }
}
