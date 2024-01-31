using UniRx;

namespace Core.Enums
{
    public abstract class InteractableEntityModel : EntityModel
    {
        public ReactiveProperty<InteractableEntityType> InteractableEntityType { get; } = new();
    }
}