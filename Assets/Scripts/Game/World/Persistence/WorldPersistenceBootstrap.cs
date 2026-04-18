using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Object = UnityEngine.Object;

namespace Game.World.Persistence
{
    public sealed class WorldPersistenceBootstrap : IDisposable
    {
        private readonly EntityActivator _activator;
        private readonly IEntityStateStore _stateStore;
        private readonly IEntityReconstructionFactory _reconstructionFactory;
        private readonly IReadOnlyList<EntityRoot> _sceneRoots;
        private readonly IPersistentSceneRootCatalog _catalog;
        private readonly IWorldStateReconciler _reconciler;

        private readonly List<EntityRoot> _activatedRoots = new();
        private readonly List<EntityRoot> _reconstructedRoots = new();

        public WorldPersistenceBootstrap(
            EntityActivator activator,
            IEntityStateStore stateStore,
            IEntityReconstructionFactory reconstructionFactory,
            IReadOnlyList<EntityRoot> sceneRoots,
            IPersistentSceneRootCatalog catalog,
            IWorldStateReconciler reconciler)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
            _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
            _reconstructionFactory =
                reconstructionFactory ?? throw new ArgumentNullException(nameof(reconstructionFactory));
            _sceneRoots = sceneRoots ?? throw new ArgumentNullException(nameof(sceneRoots));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _reconciler = reconciler ?? throw new ArgumentNullException(nameof(reconciler));
        }

        public void Initialize()
        {
            try
            {
                var persistentSceneRoots = _catalog.Collect(_sceneRoots);

                if (_stateStore.HasSnapshot == false)
                {
                    ActivateRoots(persistentSceneRoots);
                    return;
                }

                var plan = _reconciler.BuildPlan(persistentSceneRoots, _stateStore.All);
                Execute(plan);
            }
            catch
            {
                DeactivateActivatedRoots();
                DestroyReconstructedRoots();
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                DeactivateActivatedRoots();
            }
            finally
            {
                DestroyReconstructedRoots();
            }
        }

        private void Execute(WorldStateReconcilePlan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            ActivateRoots(plan.SceneRootsToActivate);

            foreach (var state in plan.StatesToReconstruct)
            {
                var root = _reconstructionFactory.Reconstruct(state);
                _reconstructedRoots.Add(root);
                Activate(root);
            }

            foreach (var root in plan.SceneRootsToRemove)
            {
                if (root == false)
                    continue;

                Object.Destroy(root.gameObject);
            }
        }

        private void ActivateRoots(IReadOnlyList<EntityRoot> roots)
        {
            if (roots == null)
                throw new ArgumentNullException(nameof(roots));

            foreach (var root in roots)
                Activate(root);
        }

        private void Activate(EntityRoot root)
        {
            _activator.Activate(root);
            _activatedRoots.Add(root);
        }

        private void DeactivateActivatedRoots()
        {
            for (var i = _activatedRoots.Count - 1; i >= 0; i--)
                _activator.Deactivate(_activatedRoots[i]);

            _activatedRoots.Clear();
        }

        private void DestroyReconstructedRoots()
        {
            for (var i = _reconstructedRoots.Count - 1; i >= 0; i--)
                _reconstructionFactory.Destroy(_reconstructedRoots[i]);

            _reconstructedRoots.Clear();
        }
    }
}