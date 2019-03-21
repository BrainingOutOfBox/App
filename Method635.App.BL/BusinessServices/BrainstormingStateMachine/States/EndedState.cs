using Method635.App.BL.BusinessServices.BrainstormingStateMachine;

namespace Method635.App.BL
{
    internal class EndedState : IState
    {
        public event ChangeStateHandler ChangeStateEvent;

        public void CleanUp()
        {
            throw new System.NotImplementedException();
        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }
    }
}