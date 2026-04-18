using System.Collections.Generic;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public abstract class UtilityActionNodePart : EntityComponent
    {
        [Header("Utility Action")] [SerializeField]
        private int _selectionOrder;

        [SerializeField, Min(0f)] private float _baseScore = 1f;
        [SerializeField, Min(0f)] private float _inertiaBonus = 0f;
        [SerializeField] private bool _isInterruptible = true;

        public override int ActivationOrder => 520;

        public int SelectionOrder => _selectionOrder;
        public float BaseScore => Mathf.Clamp01(_baseScore);
        public float InertiaBonus => Mathf.Clamp01(_inertiaBonus);
        public bool IsInterruptible => _isInterruptible;

        protected sealed override void OnActivate() => OnActionActivated();

        protected virtual void OnActionActivated()
        {
        }

        public abstract void CollectOptions(List<IUtilityOption> options);

        public virtual void BeginExecution(IUtilityOption option)
        {
        }

        public abstract UtilityActionStatus TickExecution(float deltaTime);

        public virtual void CancelExecution()
        {
        }
    }
}