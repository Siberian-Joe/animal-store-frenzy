using System;
using R3;
using UnityEngine;

namespace Game.World.PlayerInteraction.Integration
{
    public sealed class UnityPlayerWorldInput : IPlayerWorldInput, IDisposable
    {
        private readonly Camera _worldCamera;
        private readonly LayerMask _worldClickMask;
        private readonly WorldClickPlaneMode _planeMode;
        private readonly float _planeCoordinate;
        private readonly Subject<PlayerWorldClick> _clicked = new();
        private readonly IDisposable _subscription;

        public UnityPlayerWorldInput(
            Camera worldCamera,
            LayerMask worldClickMask,
            WorldClickPlaneMode planeMode,
            float planeCoordinate)
        {
            _worldCamera = worldCamera ? worldCamera : throw new ArgumentNullException(nameof(worldCamera));
            _worldClickMask = worldClickMask;
            _planeMode = planeMode;
            _planeCoordinate = planeCoordinate;

            _subscription = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => PublishClick(Input.mousePosition));
        }

        public Observable<PlayerWorldClick> Clicked => _clicked;

        public void Dispose()
        {
            _subscription.Dispose();
            _clicked.Dispose();
        }

        private void PublishClick(Vector2 screenPosition)
        {
            var ray = _worldCamera.ScreenPointToRay(screenPosition);
            var worldPosition = ResolveWorldPosition(ray);
            var hitComponent = ResolveHit(ray);
            _clicked.OnNext(new PlayerWorldClick(screenPosition, worldPosition, hitComponent, hitComponent != null));
        }

        private Vector3 ResolveWorldPosition(Ray ray)
        {
            var plane = _planeMode == WorldClickPlaneMode.XY
                ? new Plane(Vector3.forward, new Vector3(0f, 0f, _planeCoordinate))
                : new Plane(Vector3.up, new Vector3(0f, _planeCoordinate, 0f));

            return plane.Raycast(ray, out var distance)
                ? ray.GetPoint(distance)
                : ray.origin;
        }

        private Component ResolveHit(Ray ray)
        {
            var hit2D = Physics2D.GetRayIntersection(ray, float.PositiveInfinity, _worldClickMask);
            if (hit2D.collider)
                return hit2D.collider;

            return Physics.Raycast(ray, out var hit3D, float.PositiveInfinity, _worldClickMask)
                ? hit3D.collider
                : null;
        }
    }
}