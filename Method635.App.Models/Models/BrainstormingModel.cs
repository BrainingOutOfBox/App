using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Method635.App.Models.Models
{
    public class BrainstormingModel : PropertyChangedBase
    {
        private ObservableCollection<BrainSheet> _brainSheets;
        public ObservableCollection<BrainSheet> BrainSheets
        {
            get =>_brainSheets;
            set =>SetProperty(ref _brainSheets, value);
        }

        private List<BrainWave> _brainWaves;
        public List<BrainWave> BrainWaves
        {
            get => _brainWaves;
            set => SetProperty(ref _brainWaves, value);
        }

        private bool _brainWaveSent;
        public bool BrainWaveSent
        {
            get => _brainWaveSent;
            set => SetProperty(ref _brainWaveSent, value);
        }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            set => SetProperty(ref _remainingTime, value);
        }

        private int _currentSheetNr;
        public int CurrentSheetIndex
        {
            get => _currentSheetNr;
            set => SetProperty(ref _currentSheetNr, value);
        }

        private bool _commitEnabled;
        public bool CommitEnabled
        {
            get => _commitEnabled;
            set => SetProperty(ref _commitEnabled, value);
        }
    }
}
