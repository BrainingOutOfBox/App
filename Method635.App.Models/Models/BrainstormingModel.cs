using System;
using System.Collections.ObjectModel;

namespace Method635.App.Models.Models
{
    public class BrainstormingModel : PropertyChangedBase
    {
        public ObservableCollection<BrainSheet> BrainSheets { get; set; }
        public ObservableCollection<BrainWave> BrainWaves { get; set; }
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
        public int CurrentSheetNr
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
