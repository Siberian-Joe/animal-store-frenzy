using System;

namespace Game.World.EntityRuntime
{
    internal interface IEntityStateBindingBridge
    {
        Type StateType { get; }

        void BindRestoredState(IEntityStateData state);

        void BindFreshState(IEntityStateData state);
    }

    public abstract class StatefulEntityComponent<TState> : EntityComponent, IEntityStateBindingBridge
        where TState : class, IEntityStateData
    {
        private TState _state;

        protected bool HasBoundState => _state != null;

        protected TState State
        {
            get
            {
                if (_state == null)
                {
                    throw new InvalidOperationException(
                        $"{GetType().Name} has not received its persisted state slice yet.");
                }

                return _state;
            }
        }

        internal void BindRestoredState(TState state)
        {
            SetBoundState(state);
            RestoreState(state);
            OnStateReady(state);
        }

        internal void BindFreshState(TState state)
        {
            SetBoundState(state);
            InitializeFreshState(state);
            OnStateReady(state);
        }

        Type IEntityStateBindingBridge.StateType => typeof(TState);

        void IEntityStateBindingBridge.BindRestoredState(IEntityStateData state)
        {
            if (state is not TState typedState)
            {
                throw new InvalidOperationException(
                    $"Expected restored state of type '{typeof(TState).Name}' but received '{state?.GetType().Name ?? "null"}'.");
            }

            BindRestoredState(typedState);
        }

        void IEntityStateBindingBridge.BindFreshState(IEntityStateData state)
        {
            if (state is not TState typedState)
            {
                throw new InvalidOperationException(
                    $"Expected fresh state of type '{typeof(TState).Name}' but received '{state?.GetType().Name ?? "null"}'.");
            }

            BindFreshState(typedState);
        }

        private void SetBoundState(TState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        protected abstract void RestoreState(TState state);

        protected abstract void InitializeFreshState(TState state);

        protected virtual void OnStateReady(TState state)
        {
        }
    }
}
