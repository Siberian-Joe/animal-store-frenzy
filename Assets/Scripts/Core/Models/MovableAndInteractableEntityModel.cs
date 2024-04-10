using Core.Enums;
using UniRx;

namespace Core.Models
{
    public class MovableAndInteractableEntityModel : MovableEntityModel
    {
        public ReactiveProperty<InteractableEntityType> InteractableEntityType { get; } = new();
    }
}