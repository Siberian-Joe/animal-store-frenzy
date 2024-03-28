using Core.Enums;
using Interfaces.Interactions;
using UniRx;

namespace Interfaces.Core
{
    public interface IInteractableEntityViewModel : IEntityViewModel, IInteractable
    {
        IReadOnlyReactiveProperty<InteractableEntityType> InteractableEntityType { get; }
        void SetInteractableEntityType(InteractableEntityType interactableEntityType);
    }
}