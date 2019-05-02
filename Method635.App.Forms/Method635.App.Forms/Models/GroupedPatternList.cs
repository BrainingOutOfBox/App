using System.Collections.ObjectModel;

namespace Method635.App.Forms.Models
{
    public class GroupedPatternList : ObservableCollection<PatternIdeaModel>
    {
        private string _category;
        public string Category {
            get =>_category;
            set
            {
                _category = value.ToUpper();
            }
        }

    }
}
