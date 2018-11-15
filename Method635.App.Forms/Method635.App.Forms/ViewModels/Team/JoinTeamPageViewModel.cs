using Method635.App.Forms.Context;
using Prism.Commands;
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
        private readonly BrainstormingContext _context;

        public JoinTeamPageViewModel(INavigationService navigationService, BrainstormingContext context)
        {
            this._navigationService = navigationService;
            this._context = context;

            SetUpBarcodeOptions();

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

        private Result _result;
        public Result Result
        {
            get => _result;
            set
            {
                SetProperty(ref _result, value);
            }
        }

        public MobileBarcodeScanningOptions BarcodeOptions { get; private set; }
	}
}
