using NewCore.ViewModels;
using R3;
using UnityEngine;

namespace NewCore.Services.Lifecycle
{
    public interface IPlayerService
    {
        ReactiveProperty<PlayerViewModel> Player { get; }
        bool TryMovePlayer(Vector3 targetPosition);
    }
}