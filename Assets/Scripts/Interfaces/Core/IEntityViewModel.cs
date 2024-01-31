using UniRx;
using UnityEngine;

namespace Interfaces.Core
{
    public interface IEntityViewModel : IViewModel
    {
        IReadOnlyReactiveProperty<Transform> Transform { get; }
        void SetTransform(Transform transform);
    }
}