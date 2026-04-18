using System;
using System.Collections.Generic;

namespace Game.World.Shop.Customers.Flow
{
    [Serializable]
    public readonly struct CustomerSpawnRequest
    {
        public string ArchetypeId { get; }
        public IReadOnlyList<CustomerNeedState> InitialNeeds { get; }

        public CustomerSpawnRequest(
            string archetypeId,
            IReadOnlyList<CustomerNeedState> initialNeeds)
        {
            if (string.IsNullOrWhiteSpace(archetypeId))
                throw new ArgumentException("Archetype id cannot be null or whitespace.", nameof(archetypeId));

            if (initialNeeds == null || initialNeeds.Count <= 0)
                throw new ArgumentException("Initial needs cannot be null or empty.", nameof(initialNeeds));

            ArchetypeId = archetypeId.Trim();
            InitialNeeds = initialNeeds;
        }
    }
}