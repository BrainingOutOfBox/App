using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Method635.App.Forms.ViewModels.Navigation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace Method635.App.Forms.ViewModels
{
    public class BrainstormingFindingListPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _brainstormingContext;

        public BrainstormingFindingListPageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator,
            BrainstormingContext brainstormingContext)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._brainstormingContext = brainstormingContext;

            FillFindingListItems();

            this.SelectFindingCommand = new DelegateCommand(SelectFinding);
            this.CreateFindingCommand = new DelegateCommand(CreateBrainstormingFinding);
        }

        
        private async void CreateBrainstormingFinding()
        {
            await this._navigationService.NavigateAsync("NewBrainstormingPage");
        }

        private void FillFindingListItems()
        {
            var findingItems = new BrainstormingFindingRestResolver().GetAllFindingsForTeam(_brainstormingContext.CurrentBrainstormingTeam?.Id);
            // Encapsulate findings from model into findings to be displayed in listview
            FindingList = findingItems.Select(finding => new BrainstormingFindingListItem(finding)).ToList();
        }

        public DelegateCommand SelectFindingCommand { get; set; }
        public DelegateCommand CreateFindingCommand { get; }
        public DelegateCommand SwipeLeftGestureCommand { get; }
        public DelegateCommand SwipeRightGestureCommand { get; }

        private void SelectFinding()
        {
            _brainstormingContext.CurrentFinding = SelectedFinding.Finding;
            this._eventAggregator.GetEvent<RenderBrainstormingEvent>().Publish();
        }

        public BrainstormingFindingListItem SelectedFinding { get; set; }

        public string Title => "Brainstorming Findings";
        private List<BrainstormingFindingListItem> _findingList;
        public List<BrainstormingFindingListItem> FindingList
        {
            get
            {
                return _findingList;
            }
            set
            {
                SetProperty(ref _findingList, value);
            }
        }
    }
}
