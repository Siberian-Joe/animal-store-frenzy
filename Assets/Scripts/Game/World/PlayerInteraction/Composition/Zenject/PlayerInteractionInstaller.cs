using System;
using Game.World.EntityRuntime;
using Game.World.Inventory;
using Game.World.PlayerInteraction.Integration;
using Game.World.PlayerInteraction.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.World.PlayerInteraction.Composition.Zenject
{
    public sealed class PlayerInteractionInstaller : MonoInstaller
    {
        [Header("Scene References")] [SerializeField]
        private EntityRoot _playerRoot;

        [SerializeField] private Camera _worldCamera;
        [SerializeField] private LayerMask _worldClickMask = ~0;

        [Header("Click Plane")] [SerializeField]
        private WorldClickPlaneMode _clickPlaneMode = WorldClickPlaneMode.XY;

        [SerializeField] private float _clickPlaneCoordinate;

        [Header("Navigation")] [SerializeField, Min(0.01f)]
        private float _navMeshSampleRadius = 1f;

        [SerializeField] private int _navMeshAreaMask = NavMesh.AllAreas;

        public override void InstallBindings()
        {
            if (_playerRoot == false)
                throw new InvalidOperationException(
                    $"{nameof(PlayerInteractionInstaller)} requires assigned player root.");

            var worldCamera = ResolveWorldCamera();

            Container
                .Bind<EntityRoot>()
                .FromInstance(_playerRoot)
                .WhenInjectedInto<PlayerControlledActorContext>();

            Container
                .Bind<EntityRoot>()
                .FromInstance(_playerRoot)
                .WhenInjectedInto<PlayerInventoryReader>();

            Container
                .Bind<IPlayerControlledActorContext>()
                .To<PlayerControlledActorContext>()
                .AsSingle();

            Container
                .Bind<IPlayerInventoryReader>()
                .To<PlayerInventoryReader>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerFeedbackService>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<UnityPlayerWorldInput>()
                .AsSingle()
                .WithArguments(worldCamera, _worldClickMask, _clickPlaneMode, _clickPlaneCoordinate);

            Container
                .Bind<IPendingPlayerInteraction>()
                .To<PendingPlayerInteraction>()
                .AsSingle();

            Container
                .Bind<INavigationTargetResolver>()
                .To<NavMeshNavigationTargetResolver>()
                .AsSingle()
                .WithArguments(_navMeshSampleRadius, _navMeshAreaMask);

            Container
                .Bind<IPlayerClickHandler>()
                .To<PlayerInteractionClickHandler>()
                .AsSingle();

            Container
                .Bind<IPlayerClickHandler>()
                .To<PlayerMovementClickHandler>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerPrimaryInteractionFlow>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlayerInteractionArrivalExecutor>()
                .AsSingle();
        }

        private Camera ResolveWorldCamera()
        {
            if (_worldCamera)
                return _worldCamera;

            var mainCamera = Camera.main;
            if (mainCamera)
                return mainCamera;

            throw new InvalidOperationException(
                $"{nameof(PlayerInteractionInstaller)} requires assigned world camera or a scene camera tagged as MainCamera.");
        }
    }
}