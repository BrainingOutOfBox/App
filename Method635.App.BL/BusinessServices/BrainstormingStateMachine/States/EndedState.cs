using System.Collections.Generic;
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

            var currentRound = _context.CurrentFinding.CurrentRound;

            var nrOfBrainsheets = _brainstormingModel.BrainSheets.Count;
            _brainstormingModel.CurrentSheetNr = (currentRound + _positionInTeam - 1) % nrOfBrainsheets;
            var currentBrainSheet = _context.CurrentFinding.BrainSheets[_brainstormingModel.CurrentSheetNr];
            _brainstormingModel.BrainWaves = new ObservableCollection<BrainWave>(currentBrainSheet.BrainWaves);
        }

        private int _positionInTeam => _teamParticipants.IndexOf(_teamParticipants.Find(p => p.UserName.Equals(_context.CurrentParticipant.UserName)));
        private List<Participant> _teamParticipants => _context.CurrentBrainstormingTeam.Participants;
    }
}