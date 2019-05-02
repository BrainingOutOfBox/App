using Method635.App.BL.Interfaces;
using Method635.App.Forms.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Method635.App.Forms.ViewModels
{
    public class PatternPageViewModel : BindableBase
	{
        private readonly IBrainstormingService _brainstormingService;

        public PatternPageViewModel(IBrainstormingService brainstormingService)
        {
            _brainstormingService = brainstormingService;
            GroupedPatterns = new List<GroupedPatternList>();
            SetPatternList();

            ClickUrlCommand = new DelegateCommand<string>(ClickUrl);
        }

        private void ClickUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }

        private void SetPatternList()
        {
            IsDownloading = true;
            var patterns = _brainstormingService.DownloadPatternIdeas();
            var groupedPatterns = patterns.GroupBy(p => p.Category);
            foreach(var group in groupedPatterns)
            {
                var patternList = new GroupedPatternList();
                var patternIdeaModels = group.Select(p => new PatternIdeaModel(p)).ToList();
                patternIdeaModels.ForEach((p)=>
                {
                    _brainstormingService.DownloadPictureIdea(p);
                });
                patternList.AddRange(patternIdeaModels);
                patternList.Category = group.Key;
                GroupedPatterns.Add(patternList);

            }

            IsDownloading = false;
        }
        private bool _isDownloading;
        public bool IsDownloading { get => _isDownloading; set => SetProperty(ref _isDownloading, value); }
        private List<GroupedPatternList> _groupedPatterns;
        public List<GroupedPatternList> GroupedPatterns { get => _groupedPatterns; set => SetProperty(ref _groupedPatterns, value); }
        public DelegateCommand<string> ClickUrlCommand { get; }
    }
}
