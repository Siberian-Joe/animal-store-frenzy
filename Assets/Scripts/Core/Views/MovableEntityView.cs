using Core.ViewModels.MovementStrategies;
using Interfaces.Core;
using UnityEngine.AI;

namespace Core.Views
{
    public class MovableEntityView<TViewModel> : EntityView<TViewModel>
        where TViewModel : IMovableEntityViewModel
    {
        protected override void Initialize()
        {
            base.Initialize();

            if (TryGetComponent(out NavMeshAgent agent))
                ViewModel.SetMovementStrategy(new NavMeshMovementStrategy(agent, ViewModel.Speed));
        }
    }
}