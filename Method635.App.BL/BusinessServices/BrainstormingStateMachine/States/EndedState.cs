using Method635.App.BL.BusinessServices.BrainstormingStateMachine;

namespace Method635.App.BL
{
    internal class EndedState : IState
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