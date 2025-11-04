using NewCore.ViewModels.World;
using R3;
using UnityEngine;

namespace NewCore.Services.Lifecycle
{
    public interface IPlayerService
    {
        ReactiveProperty<PlayerViewModel> Player { get; }
        bool TryMovePlayer(Vector2 targetPosition);
    }
}