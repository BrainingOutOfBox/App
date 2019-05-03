using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Resources;
using Prism.Events;
using Xamarin.Forms;

namespace Method635.App.Forms.Views.Brainstorming.SpecialContent.PatternIdea
{
    public partial class PatternPage : ContentPage
    {
        public PatternPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<PatternAddedEvent>().Subscribe(() =>
            {
                DisplayAlert(AppResources.PatternAddedTitle, AppResources.PatternAddedMessage, AppResources.Ok);
            });
        }
    }
}
