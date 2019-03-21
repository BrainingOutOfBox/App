using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.Forms.Context;
using Method635.App.Forms.RestAccess;
using System.Timers;

namespace Method635.App.BL
{
    internal class WaitingState : IState
    {
        public event ChangeStateHandler ChangeStateEvent;
        private Timer _timer;
        private readonly BrainstormingFindingRestResolver _brainstormingRestResolver;
        private readonly BrainstormingContext _context;

        public WaitingState(
            BrainstormingFindingRestResolver brainstormingRestResolver,
            BrainstormingContext context)
        {
            _brainstormingRestResolver = brainstormingRestResolver;
            _context = context;
        }
        public void CleanUp()
        {
            _timer.Dispose();
        }

        public void Init()
        {
            _timer = new Timer(2500);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var backendFinding = _brainstormingRestResolver.GetFinding(_context.CurrentFinding);
            if (backendFinding.CurrentRound != _context.CurrentFinding.CurrentRound)
            {
                _context.CurrentFinding = backendFinding;
                //_logger.Info("Brainstorming has started, changing state to running");
                ChangeStateEvent?.Invoke(new RunningState());
            }
        }
    }
}