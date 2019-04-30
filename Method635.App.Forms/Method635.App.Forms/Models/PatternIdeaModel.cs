using Method635.App.Models;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Method635.App.Forms.Models
{
    public class PatternIdeaModel : BindableBase
    {
        private string _category;
        public string Category { get => _category; set => SetProperty(ref _category, value); }
        private List<PatternIdea> _patterns;
        public List<PatternIdea> Patterns { get => _patterns; set => SetProperty(ref _patterns, value); }
    }
}
