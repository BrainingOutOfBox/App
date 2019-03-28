﻿using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.BL.Interfaces;
using Method635.App.Dal.Interfaces;
using Method635.App.Forms.Context;
using Method635.App.Forms.RestAccess;
using Method635.App.Models;
using Method635.App.Models.Models;
using System;
using System.Collections.ObjectModel;

namespace Method635.App.BL
{
    public class BrainstormingService : PropertyChangedBase, IBrainstormingService
    {
        private readonly BrainstormingContext _context;
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly StateMachine _stateMachine;

        public BrainstormingService(
            IDalService iDalService,
            BrainstormingContext brainstormingContext,
            BrainstormingModel brainstormingModel)
        {
            _context = brainstormingContext;
            _brainstormingDalService = iDalService.BrainstormingDalService;
            _stateMachine = new StateMachine(_brainstormingDalService, _context, brainstormingModel);
            _stateMachine.PropertyChanged += StateMachine_PropertyChanged;

            _brainstormingModel = brainstormingModel;
            _brainstormingModel.PropertyChanged += _brainstormingModel_PropertyChanged;

            IsModerator = new TeamRestResolver().GetModeratorByTeamId(_context.CurrentBrainstormingTeam.Id).UserName.Equals(_context.CurrentParticipant.UserName);
        }

        private void _brainstormingModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            BrainWaveSent = _brainstormingModel.BrainWaveSent;
            BrainSheets = _brainstormingModel.BrainSheets;
            RemainingTime = _brainstormingModel.RemainingTime;
        }

        public void StartBusinessService()
        {
            _stateMachine.Start();
        }

        private void StateMachine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsWaiting = _stateMachine.CurrentState is WaitingState;
            IsRunning = _stateMachine.CurrentState is RunningState;
            IsEnded = _stateMachine.CurrentState is EndedState;
        }

        private bool _isWaiting;
        public bool IsWaiting
        {
            get => _isWaiting;
            set => SetProperty(ref _isWaiting, value);
        }
        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private bool _isEnded;
        public bool IsEnded
        {
            get => _isEnded;
            set => SetProperty(ref _isEnded, value);
        }


        private readonly BrainstormingModel _brainstormingModel;

        private bool _brainWaveSent;
        public bool BrainWaveSent
        {
            get => _brainWaveSent;
            set => SetProperty(ref _brainWaveSent, value);
        }
        public ObservableCollection<BrainSheet> BrainSheets { get; set; }
        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            set => SetProperty(ref _remainingTime, value);
        }

        public bool IsModerator { get; }

        public void SendBrainWave()
        {
            throw new NotImplementedException();
        }

        public void StartBrainstorming()
        {
            _brainstormingDalService.StartBrainstormingFinding(_context.CurrentFinding.Id);
            _context.CurrentFinding = _brainstormingDalService.GetFinding(_context.CurrentFinding.Id); 
        }
    }
}
