using Prism.Mvvm;
using System.Collections.Generic;

namespace Method635.App.Forms.BusinessModels
{
    public class BrainSheet : BindableBase
    {
        private int _nrOfSheet;
        public int NrOfSheet { get=>_nrOfSheet; set=>SetProperty(ref _nrOfSheet, value); }

        private List<BrainWave> _brainWaves;
        public List<BrainWave> BrainWaves { get=>_brainWaves; set=>SetProperty(ref _brainWaves, value); }
    }
}
