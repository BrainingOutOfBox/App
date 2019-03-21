using Method635.App.BL.BusinessServices.BrainstormingStateMachine;

namespace Method635.App.BL
{
    internal class RunningState : IState
    {
        public event ChangeStateHandler ChangeStateEvent;

        public void CleanUp()
        {
        }

        public void Init()
        {
        }
    }
}