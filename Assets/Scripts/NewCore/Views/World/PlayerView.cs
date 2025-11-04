using NewCore.ViewModels.World;
using UnityEngine;
using UnityEngine.AI;

namespace NewCore.Views.World
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerView : NavigableEntityView<PlayerViewModel>
    {
    }
}