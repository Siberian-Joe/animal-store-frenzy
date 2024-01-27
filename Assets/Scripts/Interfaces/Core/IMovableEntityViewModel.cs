using UniRx;

namespace Interfaces.Core
{
    public interface IMovableEntityViewModel : IEntityViewModel
    {
        IReadOnlyReactiveProperty<float> Speed { get; }
        void SetMovementStrategy(IMovementStrategy movementStrategy);
        void SetSpeed(float speed);
    }
}