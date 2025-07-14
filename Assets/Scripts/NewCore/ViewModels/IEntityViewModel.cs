using R3;
using UnityEngine;

namespace NewCore.ViewModels
{
    public interface IEntityViewModel : IViewModel
    {
        public string ID { get; }
        public ReactiveProperty<Vector3> Position { get; }
    }
}