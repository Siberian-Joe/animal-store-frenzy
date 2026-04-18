using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.UtilityAi
{
    [DisallowMultipleComponent]
    public sealed class UtilityDecisionAuthoringPart : EntityComponent
    {
        [SerializeField] private Transform _authoringRoot;

        private readonly List<ActionBinding> _bindings = new(8);

        public override int ActivationOrder => 530;

        public IReadOnlyList<ActionBinding> Bindings => _bindings;

        protected override void OnActivate() => RebuildBindings();

        public void RebuildBindings()
        {
            _bindings.Clear();

            var root = _authoringRoot ? _authoringRoot : transform;

            for (var index = 0; index < root.childCount; index++)
            {
                var child = root.GetChild(index);
                if (child == false)
                    continue;

                var action = child.GetComponent<UtilityActionNodePart>();
                if (action == false)
                    continue;

                if (action.OwnerRoot != OwnerRoot)
                    continue;

                var considerations = child.GetComponentsInChildren<UtilityConsiderationPart>(true);
                Array.Sort(considerations, static (left, right) => left.Order.CompareTo(right.Order));

                _bindings.Add(new ActionBinding(action, considerations));
            }

            if (_bindings.Count == 0)
            {
                throw new InvalidOperationException(
                    $"{nameof(UtilityDecisionAuthoringPart)} on '{name}' requires at least one direct child action.");
            }

            _bindings.Sort(static (left, right) =>
            {
                var byOrder = left.Action.SelectionOrder.CompareTo(right.Action.SelectionOrder);
                return byOrder != 0
                    ? byOrder
                    : string.CompareOrdinal(left.Action.GetType().Name, right.Action.GetType().Name);
            });
        }

        public readonly struct ActionBinding
        {
            public UtilityActionNodePart Action { get; }

            public IReadOnlyList<UtilityConsiderationPart> Considerations { get; }

            public ActionBinding(UtilityActionNodePart action, IReadOnlyList<UtilityConsiderationPart> considerations)
            {
                Action = action ?? throw new ArgumentNullException(nameof(action));
                Considerations = considerations ?? throw new ArgumentNullException(nameof(considerations));
            }
        }
    }
}