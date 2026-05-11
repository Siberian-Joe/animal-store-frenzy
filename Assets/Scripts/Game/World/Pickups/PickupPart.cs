using System;
using Game.World.EntityRuntime;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Pickups
{
    [DisallowMultipleComponent]
    public sealed class PickupPart : EntityComponent
    {
        [SerializeField] private ItemStackDefinition _contents;

        public override int ActivationOrder => 260;

        public ItemStack Contents => _contents?.ToStack() ?? throw new InvalidOperationException(
            $"{nameof(PickupPart)} on '{name}' requires pickup contents.");

        protected override void OnActivate() => _ = Contents;
    }
}