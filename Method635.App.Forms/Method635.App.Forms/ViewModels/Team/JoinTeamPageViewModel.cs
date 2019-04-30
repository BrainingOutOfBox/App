using Method635.App.BL.Context;
using Method635.App.BL.Interfaces;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Resources;
using Method635.App.Forms.Services;
using Method635.App.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;

namespace Method635.App.Forms.ViewModels.Team
{
    public class JoinTeamPageViewModel : BindableBase
	{
        private readonly IUiNavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITeamService _teamService;
        private readonly BrainstormingContext _context;

        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public JoinTeamPageViewModel(
            IUiNavigationService navigationService, 
            IEventAggregator eventAggregator, 
            ITeamService teamService,
            BrainstormingContext context)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _teamService = teamService;
            _context = context;

            FoundTeamIdCommand = new DelegateCommand<Result>(JoinTeam);

            SetUpBarcodeOptions();
            ScanForResults = true;
        }
        private void JoinTeam(Result result)
        {
            if(!string.IsNullOrEmpty(result.Text))
            {
                ScanForResults = false;
                BottomOverlayText = AppResources.BottomOverlayText;
                if(!_teamService.JoinTeam(result.Text, _context.CurrentParticipant))
                {
                    BottomOverlayText = AppResources.SomethingWrongTryAgain;
                    _logger.Error($"Couldn't join team '{result.Text}' with participant '{_context.CurrentParticipant}'.");
                    ScanForResults = true;
                    return;
                }

                BottomOverlayText += AppResources.Success;
                _context.CurrentBrainstormingTeam = _teamService.GetTeam(result.Text);

                _eventAggregator.GetEvent<RenderBrainstormingListEvent>().Publish();
            }
        }

        private void SetUpBarcodeOptions()
        {
            BarcodeOptions = new MobileBarcodeScanningOptions()
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

        private bool _scanForResults;
        public bool ScanForResults
        {
            get => _scanForResults;
            set
            {
                SetProperty(ref _scanForResults, value);
            }
        }
        public DelegateCommand<Result> FoundTeamIdCommand { get; private set; }
        public MobileBarcodeScanningOptions BarcodeOptions { get; private set; }
	}
}
