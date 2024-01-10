using System;
using Interfaces.Services;
using UniRx;
using UnityEngine;

namespace Services
{
    public class InputService : IInputService
    {
        public IObservable<Vector2> Direction => Observable.EveryUpdate()
            .Select(_ => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        public IObservable<Unit> Interact =>
            Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.E)).AsUnitObservable();
    }
}