using R3;
using UnityEngine;

namespace NewCore.Data
{
    public interface INavigableEntityProxy : IEntityProxy
    {
        ReactiveProperty<Vector3> TargetPosition { get; }
    }
}