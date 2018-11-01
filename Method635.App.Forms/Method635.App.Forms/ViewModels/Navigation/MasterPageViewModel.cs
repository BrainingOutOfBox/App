using Method635.App.Forms.RestAccess;
using Method635.App.Forms.ViewModels.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Method635.App.Forms.ViewModels
{
    public class MasterPageViewModel : BindableBase
    {

        public MasterPageViewModel()
        {
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

        }

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
