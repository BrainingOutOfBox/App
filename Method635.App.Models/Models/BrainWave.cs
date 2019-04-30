using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Method635.App.Models
{
    public class BrainWave : PropertyChangedBase
    {
        public int NrOfBrainWave { get; set; }

        private ObservableCollection<Idea> _ideas;
        public ObservableCollection<Idea> Ideas { get=>_ideas; set=>SetProperty(ref _ideas, value); }
    }
}
