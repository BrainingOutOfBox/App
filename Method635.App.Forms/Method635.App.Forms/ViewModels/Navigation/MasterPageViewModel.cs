using Method635.App.Forms.RestAccess;
using Method635.App.Forms.ViewModels.Navigation;
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
        }

        private List<BrainstormingFindingListItem> FillFindingListItems()
        {
            var findingItems = new BrainstormingFindingRestResolver().GetAllFindingsForTeam();
            return findingItems.Select(finding => new BrainstormingFindingListItem(finding)).ToList();
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
