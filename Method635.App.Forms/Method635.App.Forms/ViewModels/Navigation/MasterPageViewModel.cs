using Method635.App.Forms.Context;
using Method635.App.Forms.RestAccess;
using Method635.App.Forms.ViewModels.Navigation;
using Method635.App.Forms.Views.Brainstorming;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Method635.App.Forms.ViewModels
{
    public class MasterPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly BrainstormingContext _brainstormingContext;

        public MasterPageViewModel(INavigationService navigationService, BrainstormingContext brainstormingContext)
        {
            this._navigationService = navigationService;
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
