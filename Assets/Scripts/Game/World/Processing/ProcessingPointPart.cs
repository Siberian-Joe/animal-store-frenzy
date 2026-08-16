using System;
using Game.World.EntityRuntime;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Processing
{
    [DisallowMultipleComponent]
    public sealed class ProcessingPointPart : EntityComponent
    {
        [SerializeField] private ItemStackDefinition _input;
        [SerializeField] private ItemStackDefinition _output;

        public override int ActivationOrder => 260;

        public ItemStack Input => _input?.ToStack() ?? throw new InvalidOperationException(
            $"{nameof(ProcessingPointPart)} on '{name}' requires input contents.");

        public ItemStack Output => _output?.ToStack() ?? throw new InvalidOperationException(
            $"{nameof(ProcessingPointPart)} on '{name}' requires output contents.");

        protected override void OnActivate()
        {
            _ = Input;
            _ = Output;
        }
    }
}
