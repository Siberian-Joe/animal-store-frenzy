using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Persistence;
using Game.World.Store;
using UnityEngine;
using Zenject;
using EntityId = Game.World.EntityRuntime.EntityId;
using Random = UnityEngine.Random;

namespace Game.World.Shop.Customers.Flow
{
    public sealed class CustomerFlowService : IInitializable, ITickable
    {
        private readonly CustomerFlowConfig _config;
        private readonly ICustomerSpawnPointLocator _spawnPointLocator;
        private readonly IEntityBlueprintCatalog _entityBlueprintCatalog;
        private readonly IEntityFactory _entityFactory;
        private readonly EntityActivator _entityActivator;
        private readonly ILiveEntityRegistry _liveEntityRegistry;
        private readonly IStoreRuntimeResolver _storeResolver;

        private float _timeUntilNextSpawn;

        public CustomerFlowService(
            CustomerFlowConfig config,
            ICustomerSpawnPointLocator spawnPointLocator,
            IEntityBlueprintCatalog entityBlueprintCatalog,
            IEntityFactory entityFactory,
            EntityActivator entityActivator,
            ILiveEntityRegistry liveEntityRegistry,
            IStoreRuntimeResolver storeResolver)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _spawnPointLocator = spawnPointLocator ?? throw new ArgumentNullException(nameof(spawnPointLocator));
            _entityBlueprintCatalog =
                entityBlueprintCatalog ?? throw new ArgumentNullException(nameof(entityBlueprintCatalog));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _entityActivator = entityActivator ?? throw new ArgumentNullException(nameof(entityActivator));
            _liveEntityRegistry = liveEntityRegistry ?? throw new ArgumentNullException(nameof(liveEntityRegistry));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
        }

        public void Initialize() => _timeUntilNextSpawn = _config.GetRandomSpawnInterval();

        public void Tick()
        {
            if (_config.AutoStart == false)
                return;

            if (_storeResolver.TryGetOpenStore(out _) == false)
                return;

            if (_spawnPointLocator.HasSpawnPoints == false)
                return;

            if (GetActiveCustomerCount() >= _config.MaxActiveCustomers)
                return;

            _timeUntilNextSpawn -= Time.deltaTime;
            if (_timeUntilNextSpawn > 0f)
                return;

            SpawnCustomer();
            _timeUntilNextSpawn = _config.GetRandomSpawnInterval();
        }

        private int GetActiveCustomerCount()
        {
            var count = 0;

            foreach (var root in _liveEntityRegistry.All)
            {
                if (root == false)
                    continue;

                if (root.TryFindOwnedComponent<ICustomerNeeds>(out _))
                    count++;
            }

            return count;
        }

        private void SpawnCustomer()
        {
            if (_spawnPointLocator.TryGetRandomSpawnPoint(out var spawnPoint) == false)
                return;

            if (TryBuildSpawnRequest(out var spawnRequest) == false)
                return;

            var blueprintId = _config.CustomerBlueprintId;
            if (_entityBlueprintCatalog.TryGetPrefab(blueprintId, out var prefab) == false || prefab == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(CustomerFlowService)} could not resolve customer prefab for blueprint '{blueprintId}'.");
            }

            EntityRoot createdRoot = null;

            try
            {
                createdRoot = _entityFactory.CreateFromPrefab(
                    prefab,
                    new EntityId(Guid.NewGuid().ToString("N")),
                    spawnPoint.transform.position,
                    spawnPoint.transform.rotation);

                _entityActivator.Activate(createdRoot, blueprintId);

                var initializer = createdRoot.FindOwnedComponent<ICustomerSpawnInitializer>();
                if (initializer == null)
                {
                    throw new InvalidOperationException(
                        $"Spawned customer '{createdRoot.name}' has no {nameof(ICustomerSpawnInitializer)}.");
                }

                initializer.ApplySpawnRequest(spawnRequest);
                MarkCustomerCycleStage(CustomerCycleStage.CustomerSpawned);
            }
            catch
            {
                if (createdRoot == false)
                    throw;

                _entityActivator.RemoveState(createdRoot);
                _entityActivator.Deactivate(createdRoot);
                _entityFactory.Destroy(createdRoot);

                throw;
            }
        }

        private void MarkCustomerCycleStage(CustomerCycleStage stage)
        {
            if (_storeResolver.TryGetAnyCycleProgressWriter(out var progress))
                progress.Mark(stage);
        }

        private bool TryBuildSpawnRequest(out CustomerSpawnRequest spawnRequest)
        {
            spawnRequest = default;

            if (TryChooseArchetype(out var archetype) == false)
                return false;

            if (TryBuildInitialNeeds(archetype, out var needs) == false)
                return false;

            spawnRequest = new CustomerSpawnRequest(archetype.ArchetypeId, needs);
            return true;
        }

        private bool TryChooseArchetype(out CustomerArchetypeDefinition archetype)
        {
            if (ShouldUseTutorialArchetype())
            {
                if (TryGetTutorialArchetype(out archetype))
                    return true;

                throw new InvalidOperationException(
                    $"{nameof(CustomerFlowConfig)} requires a tutorial-only customer archetype while the tutorial customer is unresolved.");
            }

            return TryChooseRegularArchetype(out archetype);
        }

        private bool ShouldUseTutorialArchetype() =>
            _storeResolver.TryGetAnyCycleProgressWriter(out var progress) &&
            progress.IsAtLeast(CustomerCycleStage.CustomerLeft) == false;

        private bool TryGetTutorialArchetype(out CustomerArchetypeDefinition archetype)
        {
            archetype = null;

            var archetypes = _config.Archetypes;
            if (archetypes == null)
                return false;

            foreach (var candidate in archetypes)
            {
                if (candidate == false || candidate.TutorialOnly == false)
                    continue;

                if (string.IsNullOrWhiteSpace(candidate.ArchetypeId))
                    continue;

                archetype = candidate;
                return true;
            }

            return false;
        }

        private bool TryChooseRegularArchetype(out CustomerArchetypeDefinition archetype)
        {
            archetype = null;

            var archetypes = _config.Archetypes;
            if (archetypes == null || archetypes.Length == 0)
                return false;

            var totalWeight = 0;

            foreach (var candidate in archetypes)
            {
                if (IsRegularArchetype(candidate) == false)
                    continue;

                totalWeight += Mathf.Max(0, candidate.SpawnWeight);
            }

            if (totalWeight <= 0)
                return false;

            var randomValue = Random.Range(0, totalWeight);

            foreach (var candidate in archetypes)
            {
                if (IsRegularArchetype(candidate) == false)
                    continue;

                var weight = Mathf.Max(0, candidate.SpawnWeight);
                if (weight <= 0)
                    continue;

                if (randomValue < weight)
                {
                    archetype = candidate;
                    return true;
                }

                randomValue -= weight;
            }

            return false;
        }

        private static bool IsRegularArchetype(CustomerArchetypeDefinition archetype) =>
            archetype != false &&
            archetype.TutorialOnly == false &&
            string.IsNullOrWhiteSpace(archetype.ArchetypeId) == false;

        private bool TryBuildInitialNeeds(
            CustomerArchetypeDefinition archetype,
            out CustomerNeedState[] needs)
        {
            needs = Array.Empty<CustomerNeedState>();

            if (archetype == false)
                return false;

            var profiles = archetype.NeedProfiles;
            if (profiles == null || profiles.Length == 0)
                return false;

            var candidateIndices = new List<int>(profiles.Length);

            for (var index = 0; index < profiles.Length; index++)
            {
                if (IsValidNeedProfile(profiles[index]))
                    candidateIndices.Add(index);
            }

            if (candidateIndices.Count <= 0)
                return false;

            var targetNeedCount = Mathf.Min(archetype.GetRandomInitialNeedCount(), candidateIndices.Count);
            if (targetNeedCount <= 0)
                return false;

            var selectedNeeds = new List<CustomerNeedState>(targetNeedCount);

            while (selectedNeeds.Count < targetNeedCount && candidateIndices.Count > 0)
            {
                var chosenCandidateSlot = ChooseWeightedCandidateSlot(candidateIndices, profiles);
                if (chosenCandidateSlot < 0)
                    break;

                var chosenProfileIndex = candidateIndices[chosenCandidateSlot];
                candidateIndices.RemoveAt(chosenCandidateSlot);

                selectedNeeds.Add(CreateNeed(profiles[chosenProfileIndex]));
            }

            if (selectedNeeds.Count <= 0)
                return false;

            needs = selectedNeeds.ToArray();
            return true;
        }

        private static bool IsValidNeedProfile(CustomerNeedProfileEntry entry) =>
            entry.Item != false && entry.Weight > 0;

        private static int ChooseWeightedCandidateSlot(
            IReadOnlyList<int> candidateIndices,
            CustomerNeedProfileEntry[] profiles)
        {
            var totalWeight = 0;

            for (var index = 0; index < candidateIndices.Count; index++)
            {
                totalWeight += Mathf.Max(0, profiles[candidateIndices[index]].Weight);
            }

            if (totalWeight <= 0)
                return -1;

            var randomValue = Random.Range(0, totalWeight);

            for (var index = 0; index < candidateIndices.Count; index++)
            {
                var weight = Mathf.Max(0, profiles[candidateIndices[index]].Weight);
                if (weight <= 0)
                    continue;

                if (randomValue < weight)
                    return index;

                randomValue -= weight;
            }

            return -1;
        }

        private static CustomerNeedState CreateNeed(CustomerNeedProfileEntry entry)
        {
            var itemId = entry.Item.Id.Value;
            var baseNeedId = string.IsNullOrWhiteSpace(entry.NeedId)
                ? itemId
                : entry.NeedId.Trim();

            var intensity = Random.Range(
                Mathf.Clamp01(entry.MinIntensity),
                Mathf.Clamp(entry.MaxIntensity, entry.MinIntensity, 1f));

            return new CustomerNeedState
            {
                NeedId = $"{baseNeedId}_{Guid.NewGuid():N}",
                ItemId = itemId,
                Intensity = intensity
            };
        }
    }
}