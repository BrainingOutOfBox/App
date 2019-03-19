using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Resources;
using Method635.App.Forms.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;

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
