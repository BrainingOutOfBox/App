using System;

namespace Method635.App.BL.BusinessServices.BrainstormingStateMachine
{
    internal delegate void ChangeStateHandler(IState newState);
    internal interface IState 
    {
        event ChangeStateHandler ChangeStateEvent;
        void Init();
        void CleanUp();
    }
}