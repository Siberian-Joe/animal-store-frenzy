using System;
using R3;
using UnityEngine;

namespace Game.World.EntityRuntime
{
    public abstract class EntityComponent : MonoBehaviour, IEntityComponent
    {
        private CompositeDisposable _activationDisposables = new();
        private EntityRoot _ownerRoot;

        public virtual int ActivationOrder => 0;

        public bool IsActive { get; private set; }

        public EntityRoot OwnerRoot
        {
            get
            {
                _ownerRoot ??= GetComponentInParent<EntityRoot>();
                return _ownerRoot;
            }
        }

        protected CompositeDisposable ActivationDisposables => _activationDisposables;

        public void Activate()
        {
            if (IsActive)
                return;

            if (OwnerRoot == false)
            {
                throw new InvalidOperationException(
                    $"Entity component '{GetType().Name}' on '{name}' is not placed under an {nameof(EntityRoot)}.");
            }

            try
            {
                OnActivate();
                IsActive = true;
            }
            catch
            {
                _activationDisposables.Dispose();
                _activationDisposables = new CompositeDisposable();
                throw;
            }
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            try
            {
                OnDeactivate();
            }
            finally
            {
                IsActive = false;
                _activationDisposables.Dispose();
                _activationDisposables = new CompositeDisposable();
            }
        }

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnDeactivate()
        {
        }

        protected virtual void OnDestroy()
        {
            _activationDisposables.Dispose();
            _activationDisposables = new CompositeDisposable();
        }
    }
}
