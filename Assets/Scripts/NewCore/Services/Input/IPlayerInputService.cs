using R3;
using UnityEngine;

namespace NewCore.Services.Input
{
    public interface IPlayerInputService
    {
        Observable<Vector2> WorldClicked { get; }
    }
}