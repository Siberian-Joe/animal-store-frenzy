using Core.Enums;
using R3;

namespace Core.Models
{
    public class MovableAndInteractableEntityModel : MovableEntityModel
    {
        public ReactiveProperty<InteractableEntityType> InteractableEntityType { get; } = new();
    }
}