using System;
using Game.World.Persistence;

namespace Game.World.Shop.Customers
{
    [Serializable]
    public sealed class CustomerProfileState : IEntityStateData
    {
        public string ArchetypeId;

        public IEntityStateData DeepClone()
        {
            return new CustomerProfileState
            {
                ArchetypeId = ArchetypeId
            };
        }
    }
}