using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.BL.Context;
using Method635.App.Dal.Interfaces;
using Method635.App.Logging;
using Method635.App.Models;
using Method635.App.Models.Models;

namespace Method635.App.BL
{
    internal class RunningState : IState
    {
        private Timer _nextCheckRoundTimer;
        private Timer _updateRoundTimer;
        private readonly object lockObj = new object();
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly BrainstormingContext _context;
        private readonly BrainstormingModel _brainstormingModel;

        public event ChangeStateHandler ChangeStateEvent;

        private readonly ILogger _logger;

        public RunningState(
            ILogger logger,
            IBrainstormingDalService brainstormingDalService, 
            BrainstormingContext context, 
            BrainstormingModel brainstormingModel)
        {
            _logger = logger;
            _brainstormingDalService = brainstormingDalService;
            _context = context;
            _brainstormingModel = brainstormingModel;
            _brainstormingModel.PropertyChanged += _brainstormingModel_PropertyChanged;
        }

        private void _brainstormingModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_brainstormingModel.BrainWaveSent && e.PropertyName.Equals(nameof(_brainstormingModel.BrainWaveSent)))
            {
                RoundStartedTimerSetup();
            }
        }

        public void CleanUp()
        {
            _brainstormingModel.PropertyChanged -= _brainstormingModel_PropertyChanged;
            if (_nextCheckRoundTimer != null)
            {
                _nextCheckRoundTimer.Elapsed -= NextCheckRoundTimerElapsed;
                _nextCheckRoundTimer.Dispose();
            }
            if (_updateRoundTimer != null)
            {
                _updateRoundTimer.Elapsed -= UpdateRoundTime;
                _updateRoundTimer.Dispose();
            }
        }

        public void Init()
        {
            EvaluateBrainWaves();
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
            lock (lockObj)
            {
                if (remainingTime < TimeSpan.FromSeconds(1) && !_brainstormingModel.BrainWaveSent)
                {
                    SendBrainWave();
                    _updateRoundTimer.Start();
                    return;
                }
                _brainstormingModel.RemainingTime = remainingTime;
                _updateRoundTimer.Start();
            }

        }

        private void SendBrainWave()
        {
            if (_context.CurrentFinding.BrainSheets == null)
            {
                _logger.Error("Brainsheets were null, can't send brainwave!");
                throw new ArgumentException("Brainsheets on current finding can't be null");
            }
            try
            {
                var nrOfBrainsheets = _context.CurrentFinding.BrainSheets.Count;
                var currentSheet = _context.CurrentFinding.BrainSheets[(_context.CurrentFinding.CurrentRound + _positionInTeam - 1) % nrOfBrainsheets];
                if (!_brainstormingDalService.UpdateSheet(_context.CurrentFinding.Id, currentSheet))
                {
                    _logger.Error("Couldn't place brainsheet");
                }
                _brainstormingModel.BrainWaveSent = true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Error("Invalid index access!", ex);
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

            if (backendFinding?.CurrentRound == -1)
            {
                _context.CurrentFinding = backendFinding;
                ChangeStateEvent?.Invoke(new EndedState(_context, _brainstormingModel));
                return;
            }
            else if (backendFinding?.CurrentRound != _context.CurrentFinding.CurrentRound)
            {
                _context.CurrentFinding = backendFinding;
                _logger.Info("Round has changed, proceeding to next round");
                NextRound();
            }
            _nextCheckRoundTimer.Start();
        }

        private void NextRound()
        {
            _nextCheckRoundTimer.Stop();
            _nextCheckRoundTimer.Dispose();
            _brainstormingModel.BrainWaveSent = false;

            EvaluateBrainWaves();
        }
        private void EvaluateBrainWaves()
        {
            if (_context.CurrentBrainstormingTeam == null || _context.CurrentFinding == null)
            {
                return;
            }
            _brainstormingModel.BrainSheets = new ObservableCollection<BrainSheet>(_context.CurrentFinding.BrainSheets);

            var currentRound = _context.CurrentFinding.CurrentRound;

            var nrOfBrainsheets = _brainstormingModel.BrainSheets.Count;
            _brainstormingModel.CurrentSheetIndex = (currentRound + _positionInTeam - 1) % nrOfBrainsheets;
            var currentBrainSheet = _context.CurrentFinding.BrainSheets[_brainstormingModel.CurrentSheetIndex];
            _brainstormingModel.BrainWaves = currentBrainSheet.BrainWaves;
        }
    }
}