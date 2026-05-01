using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Readiness.Runtime.Contracts;
using UnityEngine.SceneManagement;

namespace Modules.Readiness.Runtime
{
    public sealed class SceneReadinessCoordinator :
        ISceneReadinessPublisher,
        ISceneReadinessAwaiter
    {
        private int _registeredSceneHandle;
        private IReadiness _registeredReadiness;

        private PendingWait _pendingWait;

        public IDisposable Register(Scene scene, IReadiness readiness)
        {
            if (scene.IsValid() == false)
                throw new ArgumentException("Scene is invalid.", nameof(scene));

            _registeredSceneHandle = scene.handle;
            _registeredReadiness = readiness ?? throw new ArgumentNullException(nameof(readiness));

            CompletePendingWaitIfMatches(scene.handle, readiness);

            return new Registration(this, scene.handle, readiness);
        }

        public async UniTask WaitReadyAsync(Scene scene, CancellationToken token)
        {
            if (scene.IsValid() == false)
                throw new ArgumentException("Scene is invalid.", nameof(scene));

            if (TryGetRegisteredReadiness(scene.handle, out var readiness))
            {
                await readiness.WaitReadyAsync(token);
                return;
            }

            var pendingWait = CreateOrGetPendingWait(scene);

            try
            {
                readiness = await pendingWait.Source.Task.AttachExternalCancellation(token);
            }
            catch
            {
                ClearPendingWaitIfSame(pendingWait);
                throw;
            }

            await readiness.WaitReadyAsync(token);
        }

        private bool TryGetRegisteredReadiness(int sceneHandle, out IReadiness readiness)
        {
            if (_registeredReadiness != null && _registeredSceneHandle == sceneHandle)
            {
                readiness = _registeredReadiness;
                return true;
            }

            readiness = null;
            return false;
        }

        private PendingWait CreateOrGetPendingWait(Scene scene)
        {
            if (_pendingWait != null)
            {
                if (_pendingWait.SceneHandle != scene.handle)
                {
                    throw new InvalidOperationException(
                        $"Scene readiness registry already waits for another scene. " +
                        $"Waiting scene handle: {_pendingWait.SceneHandle}, " +
                        $"requested scene: '{scene.name}' ({scene.handle}).");
                }

                return _pendingWait;
            }

            _pendingWait = new PendingWait(scene.handle);
            return _pendingWait;
        }

        private void CompletePendingWaitIfMatches(int sceneHandle, IReadiness readiness)
        {
            if (_pendingWait == null)
                return;

            if (_pendingWait.SceneHandle != sceneHandle)
                return;

            var pendingWait = _pendingWait;
            _pendingWait = null;

            pendingWait.Source.TrySetResult(readiness);
        }

        private void ClearPendingWaitIfSame(PendingWait pendingWait)
        {
            if (_pendingWait != pendingWait)
                return;

            _pendingWait = null;
        }

        private void Unregister(int sceneHandle, IReadiness readiness)
        {
            if (_registeredSceneHandle != sceneHandle)
                return;

            if (ReferenceEquals(_registeredReadiness, readiness) == false)
                return;

            _registeredSceneHandle = 0;
            _registeredReadiness = null;
        }

        private sealed class PendingWait
        {
            public PendingWait(int sceneHandle)
            {
                SceneHandle = sceneHandle;
            }

            public int SceneHandle { get; }

            public UniTaskCompletionSource<IReadiness> Source { get; } = new();
        }

        private sealed class Registration : IDisposable
        {
            private readonly SceneReadinessCoordinator _owner;
            private readonly int _sceneHandle;
            private readonly IReadiness _readiness;

            private bool _disposed;

            public Registration(
                SceneReadinessCoordinator owner,
                int sceneHandle,
                IReadiness readiness)
            {
                _owner = owner ?? throw new ArgumentNullException(nameof(owner));
                _sceneHandle = sceneHandle;
                _readiness = readiness ?? throw new ArgumentNullException(nameof(readiness));
            }

            public void Dispose()
            {
                if (_disposed)
                    return;

                _disposed = true;
                _owner.Unregister(_sceneHandle, _readiness);
            }
        }
    }
}