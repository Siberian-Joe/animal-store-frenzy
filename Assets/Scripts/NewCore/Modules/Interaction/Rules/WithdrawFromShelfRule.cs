using NewCore.Data;
using NewCore.Modules.Interaction.Abstractions;
using UnityEngine;

namespace NewCore.Modules.Interaction.Rules
{
    public sealed class WithdrawFromShelfRule : InteractionRule<Shelf>
    {
        protected override bool Filter(IActor initiator, Shelf shelf) => shelf.Capacity.CurrentValue > 0;

        protected override void Execute(IActor initiator, Shelf shelf)
        {
            shelf.WithdrawOne();
            Debug.Log($"Withdraw from shelf {shelf.Id} by {initiator.Id}. Current capacity: {shelf.Capacity.CurrentValue}");
        }
    }
}