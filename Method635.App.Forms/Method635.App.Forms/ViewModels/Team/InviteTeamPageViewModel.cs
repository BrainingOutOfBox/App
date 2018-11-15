using Method635.App.Forms.Context;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Drawing;
using System.IO;
using System.Timers;
using Xamarin.Forms;
using ZXing;
using ZXing.Common;
using ZXing.Net.Mobile.Forms;
using ZXing.Rendering;

namespace Method635.App.Forms.ViewModels.Team
{
	public class InviteTeamPageViewModel : BindableBase, IDestructible
	{
        private readonly BrainstormingContext _context;

        public InviteTeamPageViewModel(BrainstormingContext context)
        {
            this._context = context;
            TeamId = _context.CurrentBrainstormingTeam.Id;
            SetUpBarcodeOptions();
            InitiateMemberCountTimer();
        }

        private void InitiateMemberCountTimer()
        {
            this._timer = new Timer(5000);
            _timer.Elapsed += UpdateMemberCount;
            _timer.Start();
        }

        private void UpdateMemberCount(object sender, ElapsedEventArgs e)
        {
            try
            {
                _memberCount = new TeamRestResolver().GetTeamById(
                    _context.CurrentBrainstormingTeam.Id).CurrentNrOfParticipants;
            }
            catch (RestEndpointException ex)
            {
                Console.WriteLine($"Couldn't get updated round time from backend {ex}");
            }
        }

        private void SetUpBarcodeOptions()
        {
            this.BarcodeOptions = new EncodingOptions() { Height = 250, Width = 250, PureBarcode = true };
        }

        public void Destroy()
        {
            this._timer.Stop();
            this._timer.Dispose();
        }

        private string _teamId;
        public string TeamId
        {
            get => _teamId;
            set
            {
                SetProperty(ref _teamId, value);
            }
        }

        private string _memberCountString;
        public string MemberCountString
        {
            get => _memberCountString;
            set
            {
                SetProperty(ref _memberCountString, value);
            }
        }
        private Timer _timer;

        private int _backendNrCount;
        private int _memberCount
        {
            get => _backendNrCount;
            set
            {
                SetProperty(ref _backendNrCount, value);
                MemberCountString = $"{_memberCount} of {_teamCapacity} Members joined";
            }
        }

        private int _teamCapacity => _context.CurrentBrainstormingTeam.NrOfParticipants;

        public EncodingOptions BarcodeOptions { get; private set; }
    }
}
