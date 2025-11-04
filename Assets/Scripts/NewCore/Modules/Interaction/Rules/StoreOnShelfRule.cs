using NewCore.Data;
using NewCore.Modules.Interaction.Abstractions;
using UnityEngine;

namespace NewCore.Modules.Interaction.Rules
{
    public sealed class StoreOnShelfRule : InteractionRule<Shelf>
    {
        protected override bool Filter(IActor initiator, Shelf shelf) =>
            shelf.Capacity.CurrentValue < shelf.MaxCapacity.CurrentValue;

        protected override void Execute(IActor initiator, Shelf shelf)
        {
            shelf.StoreOne();
            Debug.Log($"Store on shelf {shelf.Id} by {initiator.Id}. Current capacity: {shelf.Capacity.CurrentValue}");
        }
    }
}