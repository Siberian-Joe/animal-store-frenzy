using System;
using System.Collections.Generic;
using System.IO;
using Game.World.EntityRuntime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Game.World.Persistence
{
    public sealed class PersistenceInstaller : MonoInstaller
    {
        [SerializeField] private PersistenceStoreMode _storeMode = PersistenceStoreMode.InMemory;
        [SerializeField] private string _stateStoreFileName = "world-state.json";
        [SerializeField] private EntityBlueprintBinding[] _entityBlueprints;

        public override void InstallBindings()
        {
            BindEntityStateStore();
            BindEntityBlueprintCatalog();

            Container
                .Bind<ILiveEntityRegistry>()
                .To<LiveEntityRegistry>()
                .AsSingle();

            Container
                .Bind<EntityRoot>()
                .FromComponentsInHierarchy()
                .AsCached();

            Container
                .Bind<IEntityComponentDiscovery>()
                .To<EntityComponentDiscovery>()
                .AsSingle();

            Container
                .Bind<IEntityStateFactory>()
                .To<ReflectionEntityStateFactory>()
                .AsSingle();

            Container
                .Bind<IEntityStateBinder>()
                .To<EntityStateBinder>()
                .AsSingle();

            Container
                .Bind<EntityActivator>()
                .AsSingle();

            Container
                .Bind<IEntityReconstructionFactory>()
                .To<EntityReconstructionFactory>()
                .AsSingle();

            Container
                .Bind<IEntityFactory>()
                .To<RuntimeEntityFactory>()
                .AsSingle();

            Container
                .Bind<IPersistentSceneRootCatalog>()
                .To<PersistentSceneRootCatalog>()
                .AsSingle();

            Container
                .Bind<IPersistentSceneRootIndex>()
                .To<PersistentSceneRootIndex>()
                .AsSingle();

            Container
                .Bind<IWorldStateReconciler>()
                .To<WorldStateReconciler>()
                .AsSingle();

            Container
                .Bind<WorldPersistenceBootstrap>()
                .AsSingle();

            Container
                .BindInterfacesTo<PersistenceBootstrapEntryPoint>()
                .AsSingle();

            Container.BindExecutionOrder<PersistenceBootstrapEntryPoint>(-300);
        }

        private void BindEntityStateStore()
        {
            switch (_storeMode)
            {
                case PersistenceStoreMode.InMemory:
                    Container
                        .Bind<IEntityStateStore>()
                        .To<InMemoryEntityStateStore>()
                        .AsSingle();
                    return;

                case PersistenceStoreMode.JsonFile:
                    Container
                        .BindInterfacesAndSelfTo<FileEntityStateStore>()
                        .AsSingle()
                        .WithArguments(ResolveStateStoreFilePath());
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(_storeMode),
                        _storeMode,
                        "Unsupported persistence store mode.");
            }
        }

        private void BindEntityBlueprintCatalog()
        {
            var prefabsById = new Dictionary<string, EntityRoot>(StringComparer.Ordinal);

            if (_entityBlueprints != null)
            {
                foreach (var binding in _entityBlueprints)
                {
                    if (string.IsNullOrWhiteSpace(binding.BlueprintId))
                    {
                        throw new InvalidOperationException(
                            $"{nameof(PersistenceInstaller)} contains an entity blueprint entry with an empty id.");
                    }

                    if (binding.Prefab == false)
                    {
                        throw new InvalidOperationException(
                            $"{nameof(PersistenceInstaller)} blueprint '{binding.BlueprintId}' has no prefab assigned.");
                    }

                    var blueprintId = binding.BlueprintId.Trim();

                    if (prefabsById.TryAdd(blueprintId, binding.Prefab) == false)
                    {
                        throw new InvalidOperationException(
                            $"{nameof(PersistenceInstaller)} contains duplicate blueprint id '{blueprintId}'.");
                    }
                }
            }

            Container
                .Bind<IEntityBlueprintCatalog>()
                .To<EntityBlueprintCatalog>()
                .AsSingle()
                .WithArguments(prefabsById);
        }

        private string ResolveStateStoreFilePath()
        {
            if (string.IsNullOrWhiteSpace(_stateStoreFileName))
            {
                throw new InvalidOperationException(
                    $"{nameof(PersistenceInstaller)} requires a non-empty state store file name when JSON persistence is enabled.");
            }

            return Path.Combine(Application.persistentDataPath, _stateStoreFileName.Trim());
        }

        [Serializable]
        private struct EntityBlueprintBinding
        {
            public string BlueprintId;
            public EntityRoot Prefab;
        }
    }
}