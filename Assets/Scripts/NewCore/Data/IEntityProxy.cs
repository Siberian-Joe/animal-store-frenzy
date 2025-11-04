using R3;
using UnityEngine;

namespace NewCore.Data
{
    public interface IEntityProxy : IProxy
    {
        string Id { get; }
        ReactiveProperty<Vector3> Position { get; }
    }
}