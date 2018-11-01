using Method635.App.Forms.ViewModels.Navigation;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Method635.App.Forms.ViewModels
{
    public class MasterPageViewModel : BindableBase
    {

        public MasterPageViewModel()
        {
            var findingsList = new List<BrainstormingFindingListItem>
            {
                new BrainstormingFindingListItem(new Models.BrainstormingFinding()
                {
                    Name = "TESTFINDING"
                })
            };
            FindingList = findingsList;
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
