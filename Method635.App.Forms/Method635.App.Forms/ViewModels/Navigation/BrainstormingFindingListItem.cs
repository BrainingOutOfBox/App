using Method635.App.Models;

namespace Method635.App.Forms.ViewModels.Navigation
{
    public class BrainstormingFindingListItem
    {
        public BrainstormingFinding Finding { get; }
        public BrainstormingFindingListItem(BrainstormingFinding finding)
        {
            Finding = finding;
        }

        public string Title { get => Finding.Name; }
    }
}
