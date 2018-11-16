using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using ZXing;
using ZXing.Common;
using ZXing.Mobile;

namespace Method635.App.Forms.ViewModels.Team
{
	public class JoinTeamPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;

        private bool _joinExecuted;
        public bool JoinPending { get => _joinExecuted;
            set
            {
                SetProperty(ref _joinExecuted, value);
            }
        }
        public JoinTeamPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, BrainstormingContext context)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._context = context;

            this.FoundTeamIdCommand = new DelegateCommand<Result>(JoinTeam);

            SetUpBarcodeOptions();
            JoinPending = false;
        }
        private void JoinTeam(Result result)
        {
            if(!JoinPending && !string.IsNullOrEmpty(result.Text))
            {
                var restResolver = new TeamRestResolver();
                BottomOverlayText = "Found Team, placing call to join it...";
                _context.CurrentParticipant = new Models.Participant() //TODO Should be taken from logged in participant
                {
                    FirstName = "Ol",
                    LastName = "Da",
                    Password = "Lassi",
                    UserName = "pqsqsi"
                };
                if(!restResolver.JoinTeam(result.Text, _context.CurrentParticipant))
                {
                    BottomOverlayText = "Something went wrong, please try again.";
                    JoinPending = true;
                    return;
                }

                BottomOverlayText += " Success!";
                JoinPending = true;
                _context.CurrentBrainstormingTeam = restResolver.GetTeamById(result.Text);

                this._eventAggregator.GetEvent<RenderBrainstormingListEvent>().Publish();
            }
        }

        private void SetUpBarcodeOptions()
        {
            this.BarcodeOptions = new MobileBarcodeScanningOptions()
            {
                TryInverted = true,
                PossibleFormats = new List<BarcodeFormat>()
                {
                    BarcodeFormat.QR_CODE
                }
            };
        }

        private string _bottomOverlayText;
        public string BottomOverlayText {
            get => _bottomOverlayText;
            set
            {
                SetProperty(ref _bottomOverlayText, value);
            }
        }

        public DelegateCommand<Result> FoundTeamIdCommand { get; private set; }
        public MobileBarcodeScanningOptions BarcodeOptions { get; private set; }
	}
}
