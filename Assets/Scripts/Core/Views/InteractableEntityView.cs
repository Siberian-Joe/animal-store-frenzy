using System;
using Interfaces.Core;
using Interfaces.Interactions;

namespace Core.Views
{
    public class InteractableEntityView<TViewModel> : EntityView<TViewModel>, IInteractable
        where TViewModel : IEntityViewModel, IInteractable
    {
        public virtual void Interact<TInteraction>(Action<TInteraction> action = null) where TInteraction : IInteraction
        {
            ViewModel.Interact(action);
        }
    }
}