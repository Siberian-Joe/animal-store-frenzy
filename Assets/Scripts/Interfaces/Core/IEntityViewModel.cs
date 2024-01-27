using UnityEngine;

namespace Interfaces.Core
{
    public interface IEntityViewModel : IViewModel
    {
        void SetTransform(Transform transform);
    }
}