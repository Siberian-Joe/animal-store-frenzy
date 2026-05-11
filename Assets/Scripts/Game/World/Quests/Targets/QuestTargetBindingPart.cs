using System;
using Game.World.EntityRuntime;
using UnityEngine;
using Zenject;

namespace Game.World.Quests.Targets
{
    [DisallowMultipleComponent]
    public sealed class QuestTargetBindingPart : EntityComponent
    {
        [SerializeField] private QuestTargetDefinition _target;

        [Inject] private readonly IQuestTargetRegistry _registry;

        public override int ActivationOrder => 340;

        protected override void OnActivate()
        {
            if (_target == false)
                throw new InvalidOperationException(
                    $"{nameof(QuestTargetBindingPart)} on '{name}' requires target definition.");

            if (_registry == null)
                throw new InvalidOperationException(
                    $"{nameof(QuestTargetBindingPart)} on '{name}' requires quest target registry.");

            _target.Validate();
            _registry.Register(_target.Id, OwnerRoot);
        }

        protected override void OnDeactivate()
        {
            if (_target && _registry != null)
                _registry.Unregister(_target.Id, OwnerRoot);
        }
    }
}