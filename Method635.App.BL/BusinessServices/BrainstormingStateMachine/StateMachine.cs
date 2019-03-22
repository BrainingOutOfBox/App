using Method635.App.Dal.Interfaces;
using Method635.App.Forms.Context;
using Method635.App.Models;
using System;

namespace Method635.App.BL.BusinessServices.BrainstormingStateMachine
{
    internal class StateMachine : PropertyChangedBase
    {
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly BrainstormingContext _context;

        public StateMachine(IBrainstormingDalService brainstormingDalService, BrainstormingContext context)
        {
            _brainstormingDalService = brainstormingDalService;
            _context = context;
        }

        public void Start()
        {
            var currentRound = _context.CurrentFinding.CurrentRound;
            IState evaluatedState = null;
            if (currentRound == -1)
            {
                evaluatedState = new EndedState();
            }
            else if (currentRound == 0)
            {
                evaluatedState = new WaitingState(_brainstormingDalService, _context);
            }
            else if (currentRound > 0)
            {
                evaluatedState = new RunningState();
            }
            if(currentRound < -1 || evaluatedState == null)
            {
                throw new ArgumentException("Invalid round or state not registered");
            }
            ChangeState(evaluatedState);
        }

        private void ChangeState(IState newState)
        {
            CurrentState?.CleanUp();
            CurrentState = newState;
            CurrentState.ChangeStateEvent -= ChangeState;
            CurrentState.ChangeStateEvent += ChangeState;
            CurrentState?.Init();
        }

        private IState _currentState;
        public IState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }
    }
}
