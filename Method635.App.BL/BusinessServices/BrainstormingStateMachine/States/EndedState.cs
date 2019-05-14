﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.BL.Context;
using Method635.App.Models;
using Method635.App.Models.Models;

namespace Method635.App.BL
{
    internal class EndedState : IState
    {
        private BrainstormingContext _context;
        private BrainstormingModel _brainstormingModel;

        public event ChangeStateHandler ChangeStateEvent;

        public EndedState(BrainstormingContext context,
            BrainstormingModel brainstormingModel)
        {
            _context = context;
            _brainstormingModel = brainstormingModel;
        }

        public void CleanUp()
        {
        }

        public void Init()
        {
            EvaluateBrainWaves();
        }

        private void EvaluateBrainWaves()
        {
            if (_context.CurrentFinding == null)
            {
                return;
            }
            _brainstormingModel.BrainSheets = new ObservableCollection<BrainSheet>(_context.CurrentFinding.BrainSheets);

            // Round has ended, simply display the first brainsheet
            _brainstormingModel.CurrentSheetIndex = 0;
            var currentBrainSheet = _context.CurrentFinding.BrainSheets[_brainstormingModel.CurrentSheetIndex];

            _brainstormingModel.BrainWaves = currentBrainSheet.BrainWaves;
        }

        private int _positionInTeam => _teamParticipants.IndexOf(_teamParticipants.Find(p => p.UserName.Equals(_context.CurrentParticipant.UserName)));
        private List<Participant> _teamParticipants => _context.CurrentBrainstormingTeam.Participants;
    }
}