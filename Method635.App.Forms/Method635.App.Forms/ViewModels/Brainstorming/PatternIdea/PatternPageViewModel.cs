using Method635.App.BL.Interfaces;
using Method635.App.Forms.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Method635.App.Forms.ViewModels
{
    public class PatternPageViewModel : BindableBase
	{
        private readonly IBrainstormingService _brainstormingService;

        public PatternPageViewModel(IBrainstormingService brainstormingService)
        {
            _brainstormingService = brainstormingService;

            Task.Run(() => SetPatternList());
        }

        private async Task SetPatternList()
        {
            IsDownloading = true;
            var patterns = await _brainstormingService.DownloadPatternIdeas();
            var groupedPatterns = patterns.GroupBy(p => p.Category);
            foreach(var patternGroup in groupedPatterns)
            {
                GroupedPatterns.Add(new PatternIdeaModel()
                {
                    Category = patternGroup.Key,
                    Patterns = patternGroup.ToList()
                });
            }
        }
        private bool _isDownloading;
        public bool IsDownloading { get => _isDownloading; set => SetProperty(ref _isDownloading, value); }
        private List<PatternIdeaModel> _groupedPatterns;
        public List<PatternIdeaModel> GroupedPatterns { get => _groupedPatterns; set => SetProperty(ref _groupedPatterns, value); }
	}
}
