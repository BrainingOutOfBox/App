using Method635.App.BL.BusinessServices.BrainstormingStateMachine;
using Method635.App.BL.Context;
using Method635.App.Dal.Interfaces;
using Method635.App.Logging;
using Method635.App.Models.Models;
using System.Timers;

namespace Method635.App.BL
{
    internal class WaitingState : IState
    {
        public event ChangeStateHandler ChangeStateEvent;
        private Timer _timer;
        private readonly IBrainstormingDalService _brainstromingDalService;
        private readonly BrainstormingContext _context;
        private readonly BrainstormingModel _brainstormingModel;
        private readonly ILogger _logger;

        public WaitingState(
            ILogger logger, 
            IBrainstormingDalService brainstromingDalService,
            BrainstormingContext context,
            BrainstormingModel brainstormingModel)
        {
            _logger = logger;
            _brainstromingDalService = brainstromingDalService;
            _context = context;
            _brainstormingModel = brainstormingModel;
        }
        public void CleanUp()
        {
            _timer.Elapsed -= Timer_Elapsed;
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
                ChangeStateEvent?.Invoke(new RunningState(_logger, _brainstromingDalService, _context, _brainstormingModel));
            }
        }
    }
}