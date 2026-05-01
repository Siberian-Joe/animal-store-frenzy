using System;
using Modules.Readiness.Runtime.Contracts;
using UnityEngine;
using Zenject;

namespace Game.SceneComposition.Shared
{
    public sealed class SceneReadinessPublication : IInitializable, IDisposable
    {
        private readonly GameObject _sceneContextObject;
        private readonly ISceneReadyGate _readyGate;
        private readonly ISceneReadinessPublisher _publisher;

        private IDisposable _registration;

        public SceneReadinessPublication(
            GameObject sceneContextObject,
            ISceneReadyGate readyGate,
            ISceneReadinessPublisher publisher)
        {
            _sceneContextObject = sceneContextObject
                ? sceneContextObject
                : throw new ArgumentNullException(nameof(sceneContextObject));

            _readyGate = readyGate ?? throw new ArgumentNullException(nameof(readyGate));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public void Initialize()
        {
            if (_registration != null)
                throw new InvalidOperationException(
                    $"{nameof(SceneReadinessPublication)} is already initialized.");

            _registration = _publisher.Register(
                _sceneContextObject.scene,
                _readyGate);
        }

        public void Dispose()
        {
            _registration?.Dispose();
            _registration = null;
        }
    }
}