using Core.Enums;
using R3;

namespace Core.Models
{
    public abstract class InteractableEntityModel : EntityModel
    {
        public ReactiveProperty<InteractableEntityType> InteractableEntityType { get; } = new();
    }
}