using System;
using UnityEngine;

namespace Interfaces.Core
{
    public interface IMovementStrategy : IDisposable
    {
        void Move(Vector2 direction);
    }
}