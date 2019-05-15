using Method635.App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Method635.App.BL.Interfaces
{
    public interface IBrainstormingService
    {
        void StartBusinessService();
        void StopBusinessService();

        void StartBrainstorming();
        Task CommitIdea(Idea idea);
        void UploadSketchIdea(SketchIdea sketchIdea, byte[] imageBytes);
        void SendBrainWave();
        Task SetPictureImageSource(Idea idea);
        List<PatternIdea> DownloadPatternIdeas();
        string GetExport();


        bool IsWaiting { get; }
        bool IsRunning { get; }
        bool IsEnded { get; }
        ObservableCollection<BrainSheet> BrainSheets { get; set; }
        bool BrainWaveSent { get; set; }
        TimeSpan RemainingTime { get; }
        bool? IsModerator { get; }
        int CurrentSheetIndex { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
