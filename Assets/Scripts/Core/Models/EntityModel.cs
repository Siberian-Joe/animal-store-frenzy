using Interfaces.Core;
using R3;
using UnityEngine;

namespace Core.Models
{
    public abstract class EntityModel : IModel
    {
        public ReactiveProperty<Transform> Transform { get; } = new();
    }
}