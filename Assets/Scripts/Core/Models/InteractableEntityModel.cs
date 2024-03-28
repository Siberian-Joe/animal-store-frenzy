using Core.Enums;
using UniRx;

namespace Core.Models
{
    public abstract class InteractableEntityModel : EntityModel
    {
        public ReactiveProperty<InteractableEntityType> InteractableEntityType { get; } = new();
    }
}