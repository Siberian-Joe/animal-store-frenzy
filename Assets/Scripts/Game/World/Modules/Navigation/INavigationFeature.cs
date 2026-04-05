using R3;
using UnityEngine;

using Game.World.Core;

namespace Game.World.Navigation
{
    public interface INavigationFeature : IEntityFeature
    {
        ReactiveProperty<bool> HasTarget { get; }
        ReactiveProperty<Vector3> TargetPosition { get; }

        Observable<Unit> Arrived { get; }

        float StoppingDistance { get; }

        void SetTarget(Vector3 targetPosition);
        void ClearTarget();
        void NotifyArrived();
    }
}
