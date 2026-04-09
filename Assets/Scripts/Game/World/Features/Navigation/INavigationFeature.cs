using Game.World.EntityRuntime;
using R3;
using UnityEngine;

namespace Game.World.Features.Navigation
{
    public interface INavigationFeature : IEntityComponent
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
