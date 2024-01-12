using System;
using Interfaces.Core;
using Interfaces.Interactions;
using Interfaces.Services.DataServices;

namespace Core.ViewModels
{
    public abstract class InteractableViewModel<TModel> : ViewModel<TModel>, IInteractable
        where TModel : IModel
    {
        protected InteractableViewModel(IDataService dataService) : base(dataService)
        {
        }

        public virtual void Interact<TInteraction>(Action<TInteraction> action = null) where TInteraction : IInteraction
        {
            if (this is TInteraction interaction)
                action?.Invoke(interaction);
        }
    }
}