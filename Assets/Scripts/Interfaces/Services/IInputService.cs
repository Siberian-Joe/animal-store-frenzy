using R3;
using UnityEngine;

namespace Interfaces.Services
{
    public interface IInputService
    {
        Observable<Vector2> Direction { get; }
        Observable<Unit> Interact { get; }
    }
}