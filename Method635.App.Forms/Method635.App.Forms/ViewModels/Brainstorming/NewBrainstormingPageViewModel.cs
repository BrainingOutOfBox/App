using Method635.App.Forms.Context;
using Method635.App.Forms.Models;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;

namespace Method635.App.Forms.ViewModels.Brainstorming
{
    public class NewBrainstormingPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;

        public DelegateCommand CreateFindingCommand { get; }

        public NewBrainstormingPageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator,
            BrainstormingContext brainstormingContext)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._context = brainstormingContext;
            this.CreateFindingCommand = new DelegateCommand(CreateFinding);
        }

        private async void CreateFinding()
        {
            var finding = new BrainstormingFinding()
            {
                Name = this.FindingName,
                NrOfIdeas = this.NrOfIdeas,
                BaseRoundTime = this.BaseRoundTime,
                ProblemDescription = this.Description
            };
            try
            {
                new BrainstormingFindingRestResolver().CreateBrainstormingFinding(finding);
                _context.CurrentFinding = finding;
                this._eventAggregator.GetEvent<RenderBrainstormingEvent>().Publish();
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Error starting brainstorming: {ex}");
            }
        }

        public string FindingName { get; set; }
        //TODO Input Validation for ints
        public int NrOfIdeas { get; set; }
        public int BaseRoundTime { get; set; }
        public string Description { get; set; }

    }
}
