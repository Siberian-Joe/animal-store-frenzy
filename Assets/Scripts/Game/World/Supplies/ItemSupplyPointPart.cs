using System;
using Game.World.EntityRuntime;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Supplies
{
    [DisallowMultipleComponent]
    public sealed class ItemSupplyPointPart : EntityComponent
    {
        [SerializeField] private ItemStackDefinition _contents;

        public override int ActivationOrder => 260;

        public ItemStack Contents => _contents?.ToStack() ?? throw new InvalidOperationException(
            $"{nameof(ItemSupplyPointPart)} on '{name}' requires supply contents.");

        protected override void OnActivate() => _ = Contents;
    }
}