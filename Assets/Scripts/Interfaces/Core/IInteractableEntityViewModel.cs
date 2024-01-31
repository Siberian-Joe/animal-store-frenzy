using Core.Enums;
using Interfaces.Core;
using Interfaces.Interactions;
using UniRx;

namespace Core.ViewModels
{
    public interface IInteractableEntityViewModel : IEntityViewModel, IInteractable
    {
        IReadOnlyReactiveProperty<InteractableEntityType> InteractableEntityType { get; }
        void SetInteractableEntityType(InteractableEntityType interactableEntityType);
    }
}