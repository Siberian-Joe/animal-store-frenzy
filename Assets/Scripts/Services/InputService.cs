using Interfaces.Services;
using R3;
using UnityEngine;

namespace Services
{
    public class InputService : IInputService
    {
        public Observable<Vector2> Direction => Observable.EveryUpdate()
            .Select(_ => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        public Observable<Unit> Interact =>
            Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.E)).AsUnitObservable();
    }
}