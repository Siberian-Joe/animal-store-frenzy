﻿using Interfaces.Core;
using UniRx;
using UnityEngine;

namespace Core.Enums
{
    public abstract class EntityModel : IModel
    {
        public ReactiveProperty<Transform> Transform { get; } = new();
    }
}