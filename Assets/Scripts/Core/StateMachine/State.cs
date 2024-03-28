using System;
using Interfaces.Core.StateMachine;
using UniRx;

namespace Core.StateMachine
{
    public abstract class State<T> : IState where T : class
    {
        public event Action<Type> StateChangeRequested;

        protected readonly T Context;
        protected readonly CompositeDisposable Disposable = new();

        protected State(T context)
        {
            Context = context;
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Exit()
        {
            foreach (var disposable in Disposable)
                disposable.Dispose();

            Disposable.Clear();
        }

        protected void ChangeState<TState>() where TState : IState
        {
            StateChangeRequested?.Invoke(typeof(TState));
        }
    }
}