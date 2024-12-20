using R3;
using UnityEngine;

namespace Interfaces.Core
{
    public interface IEntityViewModel : IViewModel
    {
        ReadOnlyReactiveProperty<Transform> Transform { get; }
        void SetTransform(Transform transform);
    }
}