using System;
using Interfaces.Core;
using Interfaces.Interactions;

namespace Core.Views
{
    public abstract class MovableAndInteractableEntityView<TViewModel> : MovableEntityView<TViewModel>, IInteractable
        where TViewModel : IMovableEntityViewModel, IInteractableEntityViewModel
    {
        public void Interact<TInteraction>(Action<TInteraction> action = null) where TInteraction : IInteraction
        {
            ViewModel.Interact(action);
        }
    }
}