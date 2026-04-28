using System;
using R3;
using UnityEngine;

namespace Game.World.EntityRuntime
{
    public abstract class EntityComponent : MonoBehaviour, IEntityComponent
    {
        public virtual int ActivationOrder => 0;

        public bool IsActive { get; private set; }

        protected CompositeDisposable ActivationDisposables { get; private set; } = new();

        public EntityRoot OwnerRoot
        {
            get
            {
                _ownerRoot ??= GetComponentInParent<EntityRoot>();
                return _ownerRoot;
            }
        }

        private EntityRoot _ownerRoot;

        public void Activate()
        {
            if (IsActive)
                return;

            if (OwnerRoot == false)
                throw new InvalidOperationException(
                    $"Entity component '{GetType().Name}' on '{name}' is not placed under an {nameof(EntityRoot)}.");

            try
            {
                OnActivate();
                IsActive = true;
            }
            catch
            {
                ActivationDisposables.Dispose();
                ActivationDisposables = new CompositeDisposable();
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
                ActivationDisposables.Dispose();
                ActivationDisposables = new CompositeDisposable();
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
            ActivationDisposables.Dispose();
            ActivationDisposables = new CompositeDisposable();
        }
    }
}