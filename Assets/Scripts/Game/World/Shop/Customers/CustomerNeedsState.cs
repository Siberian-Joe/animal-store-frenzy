using System;
using System.Collections.Generic;
using Game.World.Persistence;

namespace Game.World.Shop.Customers
{
    [Serializable]
    public sealed class CustomerNeedsState : IEntityStateData
    {
        public List<CustomerNeedState> Needs = new();

        public IEntityStateData DeepClone()
        {
            var clone = new CustomerNeedsState();

            if (Needs == null)
                return clone;

            for (var index = 0; index < Needs.Count; index++)
            {
                var need = Needs[index];
                if (need == null)
                    continue;

                clone.Needs.Add(need.DeepClone());
            }

            return clone;
        }
    }
}