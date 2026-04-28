using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Game.World.Shop.Customers.Flow
{
    public sealed class CustomerSpawnPointLocator :
        ICustomerSpawnPointRegistry,
        ICustomerSpawnPointLocator
    {
        private readonly List<CustomerSpawnPointPart> _spawnPoints = new(4);

        public bool HasSpawnPoints => _spawnPoints.Count > 0;

        public void Register(CustomerSpawnPointPart spawnPoint)
        {
            if (spawnPoint == false)
                throw new ArgumentNullException(nameof(spawnPoint));

            if (_spawnPoints.Contains(spawnPoint))
                return;

            _spawnPoints.Add(spawnPoint);
        }

        public void Unregister(CustomerSpawnPointPart spawnPoint)
        {
            if (spawnPoint == false)
                return;

            _spawnPoints.Remove(spawnPoint);
        }

        public bool TryGetRandomSpawnPoint(out CustomerSpawnPointPart spawnPoint)
        {
            CleanupDestroyedEntries();

            if (_spawnPoints.Count <= 0)
            {
                spawnPoint = null;
                return false;
            }

            var index = Random.Range(0, _spawnPoints.Count);
            spawnPoint = _spawnPoints[index];
            return spawnPoint != false;
        }

        private void CleanupDestroyedEntries()
        {
            for (var index = _spawnPoints.Count - 1; index >= 0; index--)
            {
                if (_spawnPoints[index] == false)
                    _spawnPoints.RemoveAt(index);
            }
        }
    }
}