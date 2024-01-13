using System;
using Interfaces.Core;
using Interfaces.Interactions;

namespace Core.Views
{
    public class InteractableView<TViewModel> : View<TViewModel>, IInteractable 
        where TViewModel : IViewModel, IInteractable
    {
        public virtual void Interact<TInteraction>(Action<TInteraction> action = null) where TInteraction : IInteraction
        {
            ViewModel.Interact(action);
        }
    }
}