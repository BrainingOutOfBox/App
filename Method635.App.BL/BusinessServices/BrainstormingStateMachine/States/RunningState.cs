using System;
using System.Collections.Generic;
using System.Timers;
using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.Dal.Interfaces;
using Method635.App.Forms.Context;
using Method635.App.Models;
using Method635.App.Models.Models;

namespace Method635.App.BL
{
    internal class RunningState : IState
    {
        private Timer _nextCheckRoundTimer;
        private Timer _updateRoundTimer;
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly BrainstormingContext _context;
        private BrainstormingModel _brainstormingModel;

        public event ChangeStateHandler ChangeStateEvent;

        public RunningState(IBrainstormingDalService brainstormingDalService, BrainstormingContext context, BrainstormingModel brainstormingModel)
        {
            _brainstormingDalService = brainstormingDalService;
            _context = context;
            _brainstormingModel = brainstormingModel;
        }

        public void CleanUp()
        {
            _nextCheckRoundTimer.Dispose();
            _updateRoundTimer.Dispose();
        }

        public void Init()
        {
            // TODO: * Remaining Time Timer 
            //       * Timer for round checker
            //       * Display BrainSheets accordingly
            RemainingTimeTimerSetup();
        }

        private void RemainingTimeTimerSetup()
        {
            _updateRoundTimer = new Timer(1000);
            _updateRoundTimer.Elapsed += UpdateRoundTime;
            _updateRoundTimer.Start();
        }

        private void UpdateRoundTime(object sender, ElapsedEventArgs e)
        {
            _updateRoundTimer.Stop();
            var remainingTime = _brainstormingDalService.GetRemainingTime(
                   _context.CurrentFinding.Id,
                   _context.CurrentFinding.TeamId);

            if (remainingTime < TimeSpan.FromSeconds(1) && !_brainstormingModel.BrainWaveSent)
            {
                SendBrainWave();
                return;
            }
            _brainstormingModel.RemainingTime = remainingTime;
            _updateRoundTimer.Start();
        }

        private void SendBrainWave()
        {
            if(_context.CurrentFinding.BrainSheets == null)
            {
                //Log error
                return;
            }
            try
            {
                var nrOfBrainsheets = _context.CurrentFinding.BrainSheets.Count;
                var currentSheet = _context.CurrentFinding.BrainSheets[(_context.CurrentFinding.CurrentRound + _positionInTeam - 1) % nrOfBrainsheets];
                if (!_brainstormingDalService.UpdateSheet(_context.CurrentFinding.Id, currentSheet))
                {
                    //_logger.Error("Couldn't place brainsheet");
                }
                _brainstormingModel.BrainWaveSent = true;
                RoundStartedTimerSetup();

            }
            catch (ArgumentOutOfRangeException ex)
            {
                //_logger.Error("Invalid index access!");
            }
        }

        private int _positionInTeam => _teamParticipants.IndexOf(_teamParticipants.Find(p => p.UserName.Equals(_context.CurrentParticipant.UserName)));
        private List<Participant> _teamParticipants => _context.CurrentBrainstormingTeam.Participants;


        private void RoundStartedTimerSetup()
        {
            _nextCheckRoundTimer = new Timer(2500);
            _nextCheckRoundTimer.Elapsed += NextCheckRoundTimerElapsed;
            _nextCheckRoundTimer.Start();
        }

        private void NextCheckRoundTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateRound();
        }
        private void UpdateRound()
        {
            _nextCheckRoundTimer.Stop();
            var backendFinding = _brainstormingDalService.GetFinding(_context.CurrentFinding.Id);
            if (backendFinding?.CurrentRound != _context.CurrentFinding.CurrentRound)
            {
                if (backendFinding?.CurrentRound == -1)
                {
                    ChangeStateEvent?.Invoke(new EndedState());
                    _nextCheckRoundTimer.Dispose();
                }
                _context.CurrentFinding = backendFinding;
                //_logger.Info("Round has changed, proceeding to next round");
                //NextRound();
            }
            _nextCheckRoundTimer.Start();
        }
    }
}