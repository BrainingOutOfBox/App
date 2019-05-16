using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Resources;
using Prism.Events;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Method635.App.Forms.Views.Team
{
    public partial class InviteTeamPage : ContentPage
    {
        public InviteTeamPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.GetEvent<InviteTeamCompleteEvent>().Subscribe(async () =>
            {
                await Task.WhenAll(
                    contentView.FadeTo(0.8, 400),
                    contentView.ScaleTo(1.2, 300).ContinueWith(async (r)=>await contentView.ScaleTo(1,100)),
                    label.FadeTo(0.8, 400),
                    label.ScaleTo(1, 400)
                    );
            });
        }

    }
}
