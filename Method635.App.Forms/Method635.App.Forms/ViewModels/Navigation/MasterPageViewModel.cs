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
    public class MasterPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _brainstormingContext;

        public MasterPageViewModel(INavigationService navigationService,
            IEventAggregator eventAggregator,
            BrainstormingContext brainstormingContext)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._brainstormingContext = brainstormingContext;

            List<BrainstormingFindingListItem> findingsList = FillFindingListItems();
            FindingList = findingsList;
            this.SelectFindingCommand = new DelegateCommand(SelectFinding);
        }

        private List<BrainstormingFindingListItem> FillFindingListItems()
        {
            var findingItems = new BrainstormingFindingRestResolver().GetAllFindingsForTeam();
            // Encapsulate findings from model into findings to be displayed in listview
            return findingItems.Select(finding => new BrainstormingFindingListItem(finding)).ToList();
        }

        public DelegateCommand SelectFindingCommand { get; set; }
        
        private void SelectFinding()
        {
            _brainstormingContext.CurrentFinding = SelectedFinding.Finding;
            this._eventAggregator.GetEvent<RenderBrainstormingEvent>().Publish();
        }

        public BrainstormingFindingListItem SelectedFinding { get; set; }


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
