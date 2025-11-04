using R3;
using UnityEngine;

namespace NewCore.ViewModels.World
{
    public interface IEntityViewModel : IViewModel
    {
        public string Id { get; }
        public ReactiveProperty<Vector3> Position { get; }
    }
}