using System;

namespace Interfaces.Interactions
{
    public interface IInteractable
    {
        void Interact<TInteraction>(Action<TInteraction> action = null) where TInteraction : IInteraction;
    }
}