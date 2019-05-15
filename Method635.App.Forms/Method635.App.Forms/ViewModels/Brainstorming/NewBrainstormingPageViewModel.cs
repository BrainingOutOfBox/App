using Method635.App.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using Method635.App.Logging;
using Method635.App.Forms.Resources;
using Method635.App.Forms.Services;
using Method635.App.Dal.Interfaces;
using Method635.App.BL.Context;
using Prism.Navigation;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class NewBrainstormingPageViewModel : BindableBase
    {
        private readonly ILogger _logger;
        private readonly IUiNavigationService _navigationService;
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly BrainstormingContext _context;
        private int _nrOfIdeas;
        private int _baseRoundTime;

        private readonly List<char> disallowedChars = new List<char> { '\\', ' ' };

        public DelegateCommand CreateFindingCommand { get; }

        public NewBrainstormingPageViewModel(
            ILogger logger,
            IUiNavigationService navigationService,
            IDalService dalService,
            BrainstormingContext brainstormingContext)
        {
            _logger = logger;
            _navigationService = navigationService;
            _brainstormingDalService = dalService.BrainstormingDalService;
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
                    ProblemDescription = Description,
                    Category = Category
                };
                finding = _brainstormingDalService.CreateFinding(finding);
                _context.CurrentFinding = finding;
                _navigationService.NavigateToBrainstormingTab(new NavigationParameters());
            }
            catch (Exception ex)
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
            if (string.IsNullOrEmpty(FindingName) ||
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
            if (baseRoundTime < 1 || baseRoundTime > 100)
            {
                ErrorText = AppResources.InvalidRoundTime;
                HasError = true;
                return false;
            }
            if (nrOfIdeas < 1 || nrOfIdeas > 99)
            {
                ErrorText = AppResources.InvalidNrOfIdeas;
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
        public string NrOfIdeasText { get; set; }
        public string BaseRoundTimeText { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; }
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
        public string ProhibitedChars => string.Format(AppResources.ProhibitedCharsText, string.Join(", ", disallowedChars));

        private bool _hasInvalidChars;
        public bool HasInvalidChars
        {
            get => _hasInvalidChars;
            set => SetProperty(ref _hasInvalidChars, value);
        }
    }
}
