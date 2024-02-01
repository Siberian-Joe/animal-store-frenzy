using System;

namespace Core.StateMachine
{
    public interface IStateMachine
    {
        void Update();
        void FixedUpdate();
        void ChangeState<TState>() where TState : IState;
        void ChangeState(Type stateType);
    }
}