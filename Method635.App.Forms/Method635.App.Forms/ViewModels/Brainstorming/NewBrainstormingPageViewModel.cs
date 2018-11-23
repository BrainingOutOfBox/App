using Method635.App.Forms.BusinessModels;
using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class NewBrainstormingPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;

        private readonly List<char> disallowedChars = new List<char>{ '\\', ' ' };

        public DelegateCommand CreateFindingCommand { get; }

        public NewBrainstormingPageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator,
            BrainstormingContext brainstormingContext)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._context = brainstormingContext;
            this.CreateFindingCommand = new DelegateCommand(CreateFinding);

            this.HasInvalidChars = false;
        }

        private void CreateFinding()
        {
            if (!CheckInput())
            {
                Console.WriteLine("Invalid input to create finding..");
                return;
            }
            try
            {
                var finding = new BrainstormingFinding()
                {
                    TeamId = _context.CurrentBrainstormingTeam.Id,
                    Name = this.FindingName,
                    NrOfIdeas = this.nrOfIdeas,
                    BaseRoundTime = this.baseRoundTime,
                    ProblemDescription = this.Description
                };
                finding = new BrainstormingFindingRestResolver().CreateBrainstormingFinding(finding);
                _context.CurrentFinding = finding;
                this._eventAggregator.GetEvent<RenderBrainstormingEvent>().Publish();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private bool CheckInput()
        {
            if (!HasValidFindingName())
            {
                this.ErrorText = "Please don't use any of the prohibited characters";
                this.HasError = true;
                this.HasInvalidChars = true;
                return false;
            }
            if(string.IsNullOrEmpty(FindingName) ||
                string.IsNullOrEmpty(NrOfIdeasText) ||
                string.IsNullOrEmpty(BaseRoundTimeText))
            {
                this.ErrorText = "Please fill in all the necessary fields";
                this.HasError = true;
                return false;
            }
            if (!int.TryParse(NrOfIdeasText, out int nrOfIdeas) ||
                !int.TryParse(BaseRoundTimeText, out int baseRoundTime))
            {
                this.ErrorText = "Please use numbers in the corresponding fields";
                this.HasError = true;
                return false;
            }
            this.baseRoundTime = baseRoundTime;
            this.nrOfIdeas = nrOfIdeas;
            return true;
        }

        private bool HasValidFindingName()
        {
            this.HasInvalidChars = false;
            return disallowedChars.TrueForAll(c => FindingName.IndexOf(c) < 0);
        }

        public string FindingName { get; set; }
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
        private int baseRoundTime;
        private int nrOfIdeas;

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }
        public string ProhibitedChars => $"Prohibited characters (comma-separated, including whitespace): {string.Join(", ", disallowedChars)}";



        private bool _hasInvalidChars;
        public bool HasInvalidChars
        {
            get => _hasInvalidChars;
            set => SetProperty(ref _hasInvalidChars, value);
        }

    }
}
