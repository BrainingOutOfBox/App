﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.Dal.Interfaces;
using Method635.App.Forms.Context;
using Method635.App.Logging;
using Method635.App.Models;
using Method635.App.Models.Models;
using Xamarin.Forms;

namespace Method635.App.BL
{
    internal class RunningState : IState
    {
        private Timer _nextCheckRoundTimer;
        private Timer _updateRoundTimer;
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly BrainstormingContext _context;
        private readonly BrainstormingModel _brainstormingModel;

        public event ChangeStateHandler ChangeStateEvent;

        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

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
            RetrieveFinding();
            RemainingTimeTimerSetup();
        }

        private void RetrieveFinding()
        {
            var retrievedFinding = _brainstormingDalService.GetFinding(_context.CurrentFinding.Id);
            if (retrievedFinding == null)
            {
                _logger.Error($"Finding retrieved from backend was null ({_context.CurrentFinding.Id})");
                throw new ArgumentException("Finding retrieved from backend can't be null.");
            }
            _context.CurrentFinding = retrievedFinding;

            // TODO: Define CurrentSheetText in vm
            // CurrentSheetText = string.Format(AppResources.SheetNrOfNr, CurrentSheetNr, _context.CurrentBrainstormingTeam.NrOfParticipants);
            EvaluateBrainWaves();
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
                RoundStartedTimerSetup();

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
            if (backendFinding?.CurrentRound != _context.CurrentFinding.CurrentRound)
            {
                if (backendFinding?.CurrentRound == -1)
                {
                    ChangeStateEvent?.Invoke(new EndedState());
                    return;
                }
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

            // TODO Handle commitideaindex in VM
            //commitIdeaIndex = 0;
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
            _brainstormingModel.CurrentSheetNr = (currentRound + _positionInTeam - 1) % nrOfBrainsheets;
            var currentBrainSheet = _context.CurrentFinding.BrainSheets[_brainstormingModel.CurrentSheetNr];
            _brainstormingModel.BrainWaves = new ObservableCollection<BrainWave>(currentBrainSheet.BrainWaves);

            // TODO: Handle commitenabled in VM?
            _brainstormingModel.CommitEnabled = _brainstormingModel.BrainWaves != null || _context.CurrentFinding?.CurrentRound > 0;
        }
    }
}