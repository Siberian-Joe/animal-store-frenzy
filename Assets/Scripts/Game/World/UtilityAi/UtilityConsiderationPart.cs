using System;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public abstract class UtilityConsiderationPart : EntityComponent
    {
        [Header("Utility Consideration")] [SerializeField]
        private int _order;

        [SerializeField, Range(0f, 1f)] private float _weight = 1f;

        [SerializeField] private AnimationCurve _responseCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        private bool _isBound;

        public override int ActivationOrder => 510;

        public int Order => _order;

        public void Bind(UtilityFactRuntime facts)
        {
            if (facts == null)
                throw new ArgumentNullException(nameof(facts));

            OnBind(facts);
            _isBound = true;
        }

        public float Evaluate()
        {
            if (_isBound == false)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} on '{name}' was evaluated before fact binding.");
            }

            var raw = Mathf.Clamp01(Measure());
            var shaped = Mathf.Clamp01(_responseCurve.Evaluate(raw));

            return Mathf.Lerp(1f, shaped, _weight);
        }

        protected sealed override void OnActivate()
        {
            if (_responseCurve is not { length: > 0 })
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} on '{name}' requires a valid response curve.");
            }

            OnConsiderationActivated();
        }

        protected virtual void OnConsiderationActivated()
        {
        }

        protected abstract void OnBind(UtilityFactRuntime facts);

        protected abstract float Measure();
    }
}