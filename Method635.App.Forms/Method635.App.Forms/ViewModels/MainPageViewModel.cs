using Method635.App.Forms.Context;
using Method635.App.Forms.Resources;
using Prism.Mvvm;

namespace Method635.App.Forms.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public MainPageViewModel(BrainstormingContext context)
        {
            Title = $"{AppResources.Method635} - {AppResources.Team} '{context.CurrentBrainstormingTeam.Name}'";
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
