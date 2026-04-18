using System;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
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

        private void BindRestoredState(TState state)
        {
            SetBoundState(state);
            RestoreState(state);
        }

        private void BindFreshState(TState state)
        {
            SetBoundState(state);
            InitializeFreshState(state);
        }

        protected sealed override void OnActivate()
        {
            ApplyBoundState(State);
            OnStateActivated();
        }

        StateSlotKey IEntityStateBindingBridge.StateSlotKey => GetStateSlotKey();
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

        protected virtual StateSlotKey GetStateSlotKey() => StateSlotKey.For(typeof(TState));

        protected virtual void ApplyBoundState(TState state)
        {
        }

        protected virtual void OnStateActivated()
        {
        }
    }
}
