using Interfaces.Core;
using UniRx;
using UnityEngine;

namespace Core.Models
{
    public class CameraModel : IModel
    {
        public ReactiveProperty<Vector3> TargetPosition { get; } = new(Vector3.zero);
        public Vector3 Offset { get; set; }
    }
}