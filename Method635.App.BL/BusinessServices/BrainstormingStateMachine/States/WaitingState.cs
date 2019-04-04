using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.BL.Context;
using Method635.App.Dal.Interfaces;
using Method635.App.Logging;
using Method635.App.Models.Models;
using System.Timers;
using Xamarin.Forms;

namespace Method635.App.BL
{
    internal class WaitingState : IState
    {
        public event ChangeStateHandler ChangeStateEvent;
        private Timer _timer;
        private readonly IBrainstormingDalService _brainstromingDalService;
        private readonly BrainstormingContext _context;
        private readonly ILogger _logger = DependencyService.Get<ILogManager>().GetLog();

        public WaitingState(
            IBrainstormingDalService brainstromingDalService,
            BrainstormingContext context)
        {
            _brainstromingDalService = brainstromingDalService;
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
            var backendFinding = _brainstromingDalService.GetFinding(_context.CurrentFinding.Id);
            if (backendFinding?.CurrentRound != _context.CurrentFinding.CurrentRound)
            {
                _context.CurrentFinding = backendFinding;
                _logger.Info("Brainstorming has started, changing state to running");
                ChangeStateEvent?.Invoke(new RunningState(_brainstromingDalService, _context, new BrainstormingModel()));
            }
        }
    }
}