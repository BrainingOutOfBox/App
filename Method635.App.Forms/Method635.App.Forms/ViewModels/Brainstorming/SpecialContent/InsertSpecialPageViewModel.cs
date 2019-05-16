using Method635.App.Forms.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace Method635.App.Forms.ViewModels
{
    public class InsertSpecialPageViewModel : BindableBase
    {

        private readonly IUiNavigationService _navigationService;

        public DelegateCommand SketchIdeaCommand { get; }
        public DelegateCommand PatternIdeaCommand { get; }
        public InsertSpecialPageViewModel(IUiNavigationService navigationService)
        {
            _navigationService = navigationService;
            SketchIdeaCommand = new DelegateCommand(ChooseSketchIdea);
            PatternIdeaCommand = new DelegateCommand(ChoosePatternIdea);
        }

        private void ChoosePatternIdea()
        {
            _navigationService.NavigateToInsertPattern();
        }

        private void ChooseSketchIdea()
        {
            _navigationService.NavigateToInsertSketch();
        }
    }
}
