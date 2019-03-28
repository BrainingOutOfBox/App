using Method635.App.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Method635.App.BL.Interfaces
{
    public interface IBrainstormingService
    {
        void StartBusinessService();
        bool IsWaiting { get; }
        bool IsRunning { get; }
        bool IsEnded { get; }
        bool BrainWaveSent { get; }
        ObservableCollection<BrainSheet> BrainSheets { get; }
        TimeSpan RemainingTime { get; }
        bool IsModerator { get; }

        void SendBrainWave();
        void StartBrainstorming();
        event PropertyChangedEventHandler PropertyChanged;
    }
}
