﻿using Method635.App.Forms.Context;
using Method635.App.Forms.PrismEvents;
using Method635.App.Forms.Resources;
using Method635.App.Forms.RestAccess;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
using ZXing;
using ZXing.Mobile;

namespace Method635.App.Forms.ViewModels.Team
{
    public class JoinTeamPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly BrainstormingContext _context;

      
        public JoinTeamPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, BrainstormingContext context)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _context = context;

            FoundTeamIdCommand = new DelegateCommand<Result>(JoinTeam);

            SetUpBarcodeOptions();
            JoinPending = false;
        }
        private void JoinTeam(Result result)
        {
            if(!JoinPending && !string.IsNullOrEmpty(result.Text))
            {
                var restResolver = new TeamRestResolver();
                BottomOverlayText = AppResources.BottomOverlayText;
                if(!restResolver.JoinTeam(result.Text, _context.CurrentParticipant))
                {
                    BottomOverlayText = AppResources.SomethingWrongTryAgain;
                    JoinPending = true;
                    return;
                }

                BottomOverlayText += AppResources.Success;
                JoinPending = true;
                _context.CurrentBrainstormingTeam = restResolver.GetTeamById(result.Text);

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

        private bool _joinPending;
        public bool JoinPending
        {
            get => _joinPending;
            set
            {
                SetProperty(ref _joinPending, value);
            }
        }
        public DelegateCommand<Result> FoundTeamIdCommand { get; private set; }
        public MobileBarcodeScanningOptions BarcodeOptions { get; private set; }
	}
}
