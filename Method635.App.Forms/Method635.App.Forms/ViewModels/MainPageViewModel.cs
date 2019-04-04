using Method635.App.BL.Context;
using Method635.App.Forms.Resources;
using Prism.Mvvm;

namespace Method635.App.Forms.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public MainPageViewModel(BrainstormingContext context)
        {
            if (context.CurrentBrainstormingTeam != null)
            {
                Title = $"{AppResources.Method635} - {AppResources.Team} '{context.CurrentBrainstormingTeam.Name}'";
                return;
            }
            Title = $"{AppResources.Method635}";
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
