using Method635.App.BL.BusinessServices;
using Method635.App.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Method635.App.BL.Interfaces
{
    public interface IBrainstormingService
    {
        void StartBusinessService();
        void StopBusinessService();
        bool IsWaiting { get; }
        bool IsRunning { get; }
        bool IsEnded { get; }
        bool BrainWaveSent { get; set; }
        ObservableCollection<BrainSheet> BrainSheets { get; set; }
        TimeSpan RemainingTime { get; }
        bool? IsModerator { get; }
        int CurrentSheetIndex { get; }
        void CommitIdea(string ideaText);
        void SendBrainWave();
        void StartBrainstorming();
        void UploadSketchIdea(SketchIdea sketchIdea, byte[] imageBytes);
        event PropertyChangedEventHandler PropertyChanged;

        Task DownloadPictureIdea(Idea idea);
    }
}
