using System.Collections.Generic;

namespace Method635.App.Forms.Models
{
    public class GroupedPatternList : List<PatternIdeaModel>
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
