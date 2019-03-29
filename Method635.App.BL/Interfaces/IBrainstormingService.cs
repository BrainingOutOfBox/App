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
        bool BrainWaveSent { get; set; }
        ObservableCollection<BrainSheet> BrainSheets { get; set; }
        TimeSpan RemainingTime { get; }
        bool? IsModerator { get; }
        int CurrentSheetNr { get; }
        void CommitIdea(string ideaText);
        void SendBrainWave();
        void StartBrainstorming();
        event PropertyChangedEventHandler PropertyChanged;

    }
}
