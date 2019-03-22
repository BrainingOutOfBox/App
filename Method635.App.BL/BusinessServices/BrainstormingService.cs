using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.Dal.Interfaces;
using Method635.App.Forms.Context;
using Method635.App.Models;

namespace Method635.App.BL
{
    public class BrainstormingService : PropertyChangedBase
    {
        private BrainstormingContext _context;
        private readonly IBrainstormingDalService _brainstormingDalService;
        private readonly StateMachine _stateMachine;

        public BrainstormingService(IBrainstormingDalService brainstormingDalService, BrainstormingContext brainstormingContext)
        {
            _context = brainstormingContext;
            _brainstormingDalService = brainstormingDalService;
            _stateMachine = new StateMachine(_brainstormingDalService, _context);
            _stateMachine.PropertyChanged += StateMachine_PropertyChanged;
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
            get=>_isWaiting;
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
    }
}
