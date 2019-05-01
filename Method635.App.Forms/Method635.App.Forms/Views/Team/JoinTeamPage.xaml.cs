using System;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Resources;
using Prism.Events;
using Xamarin.Forms;

namespace Method635.App.Forms.Views.Team
{
    public partial class JoinTeamPage : ContentPage
    {
        private readonly IEventAggregator _eventAggregator;

        public JoinTeamPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _eventAggregator.GetEvent<RenderBrainstormingListEvent>().Subscribe(async () =>
                {
                    await DisplayAlert(AppResources.JoinedTeamTitle, AppResources.JoinedTeamMessage, AppResources.Ok);
                }
            );
        }
    }
}
