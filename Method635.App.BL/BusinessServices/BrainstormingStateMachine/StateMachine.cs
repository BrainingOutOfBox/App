using Method635.App.BL.Context;
using Method635.App.Dal.Interfaces;
using Method635.App.Logging;
using Method635.App.Models;
using Method635.App.Models.Models;
using System;

namespace Method635.App.BL.BusinessServices.BrainstormingStateMachine
{
    internal class StateMachine : PropertyChangedBase
    {
        private readonly ILogger _logger;
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly BrainstormingContext _context;
        private readonly BrainstormingModel _brainstormingModel;

        public StateMachine(
            ILogger logger,
            IBrainstormingDalService brainstormingDalService, 
            BrainstormingContext context,
            BrainstormingModel brainstormingModel)
        {
            _logger = logger;
            _brainstormingDalService = brainstormingDalService;
            _context = context;
            _brainstormingModel = brainstormingModel;
        }

        public void Start()
        {
            var currentRound = _context.CurrentFinding.CurrentRound;
            IState evaluatedState = null;
            if (currentRound == -1)
            {
                evaluatedState = new EndedState(_context, _brainstormingModel);
            }
            else if (currentRound == 0)
            {
                evaluatedState = new WaitingState(_logger, _brainstormingDalService, _context);
            }
            else if (currentRound > 0)
            {
                evaluatedState = new RunningState(_logger, _brainstormingDalService, _context, _brainstormingModel);
            }
            if(currentRound < -1 || evaluatedState == null)
            {
                throw new ArgumentException("Invalid round or state not registered");
            }
            ChangeState(evaluatedState);
        }

        public void Stop()
        {
            if (CurrentState != null)
            {
                CurrentState.CleanUp();
                CurrentState.ChangeStateEvent -= ChangeState;
            }
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
            private set => SetProperty(ref _currentState, value);
        }
    }
}
