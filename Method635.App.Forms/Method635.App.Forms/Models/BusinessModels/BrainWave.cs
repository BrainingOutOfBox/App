using Prism.Mvvm;
using System.Collections.Generic;

namespace Method635.App.Forms.BusinessModels
{
    public class BrainWave : BindableBase
    {
        private int _nrOfBrainWave;
        public int NrOfBrainWave { get => _nrOfBrainWave; set => SetProperty(ref _nrOfBrainWave, value); }

        private List<TextIdea> _ideas;
        public List<TextIdea> Ideas { get => _ideas; set => SetProperty(ref _ideas, value); }
    }
}
