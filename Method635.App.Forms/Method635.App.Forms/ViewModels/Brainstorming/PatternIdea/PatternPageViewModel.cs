using Method635.App.BL.Interfaces;
using Method635.App.Forms.Models;
using Method635.App.Forms.PrismEvents;
using Prism.Commands;
using Prism.Events;
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
        private readonly IEventAggregator _eventAggregator;

        public PatternPageViewModel(IBrainstormingService brainstormingService, IEventAggregator eventAggregator)
        {
            _brainstormingService = brainstormingService;
            _eventAggregator = eventAggregator;
            GroupedPatterns = new List<GroupedPatternList>();
            SetPatternList();

            ClickUrlCommand = new DelegateCommand<string>(ClickUrl);
            ClickPatternCommand = new DelegateCommand<PatternIdeaModel>(ClickPattern);
        }

        private void ClickPattern(PatternIdeaModel patternIdea)
        {
            _brainstormingService.CommitIdea(patternIdea);
            _eventAggregator.GetEvent<PatternAddedEvent>().Publish();
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
                    _brainstormingService.SetPictureImageSource(p);
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
        public DelegateCommand<PatternIdeaModel> ClickPatternCommand { get; }
    }
}
