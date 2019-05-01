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
            GroupedPatterns = new List<PatternIdeaModel>();

            Task.Run(() => SetPatternList());
            ClickUrlCommand = new DelegateCommand<string>(ClickUrl);
        }

        private void ClickUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }

        private async Task SetPatternList()
        {
            IsDownloading = true;
            var patterns = await _brainstormingService.DownloadPatternIdeas();
            GroupedPatterns.AddRange(patterns.Select(p => new PatternIdeaModel(p)));
            IsDownloading = false;
        }
        private bool _isDownloading;
        public bool IsDownloading { get => _isDownloading; set => SetProperty(ref _isDownloading, value); }
        private List<PatternIdeaModel> _groupedPatterns;
        public List<PatternIdeaModel> GroupedPatterns { get => _groupedPatterns; set => SetProperty(ref _groupedPatterns, value); }
        public DelegateCommand<string> ClickUrlCommand { get; }
    }
}
