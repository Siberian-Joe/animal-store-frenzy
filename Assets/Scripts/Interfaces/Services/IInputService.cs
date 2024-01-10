using System;
using UniRx;
using UnityEngine;

namespace Interfaces.Services
{
    public interface IInputService
    {
        IObservable<Vector2> Direction { get; }
        IObservable<Unit> Interact { get; }
    }
}