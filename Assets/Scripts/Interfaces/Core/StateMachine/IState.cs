using System;

namespace Core.StateMachine
{
    public interface IState
    {
        event Action<Type> StateChangeRequested;
        void Enter();
        void Update();
        void FixedUpdate();
        void Exit();
    }
}