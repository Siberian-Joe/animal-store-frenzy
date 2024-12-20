using Core.Enums;
using Interfaces.Interactions;
using R3;

namespace Interfaces.Core
{
    public interface IInteractableEntityViewModel : IEntityViewModel, IInteractable
    {
        ReadOnlyReactiveProperty<InteractableEntityType> InteractableEntityType { get; }
        void SetInteractableEntityType(InteractableEntityType interactableEntityType);
    }
}