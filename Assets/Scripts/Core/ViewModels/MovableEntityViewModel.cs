using Core.Enums;
using Interfaces.Core;
using Interfaces.Services.DataServices;
using UniRx;
using UnityEngine;

namespace Core.ViewModels
{
    public abstract class MovableEntityViewModel<TModel> : EntityViewModel<TModel>, IMovableEntityViewModel where TModel : MovableEntityModel
    {
        public IReadOnlyReactiveProperty<float> Speed => Model.Speed;
        public IReadOnlyReactiveProperty<Vector2> TargetPosition => Model.TargetPosition;
        public IReadOnlyReactiveProperty<bool> IsMoving => Model.IsMoving;

        protected IMovementStrategy MovementStrategy { get; set; }

        protected MovableEntityViewModel(IDataService dataService) : base(dataService)
        {
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsMoving.Value == false || MovementStrategy == null)
                return;

            MovementStrategy.Move(TargetPosition.Value);
        }
        
        public void SetMovementStrategy(IMovementStrategy movementStrategy)
        {
            MovementStrategy = movementStrategy;
        }

        public void SetSpeed(float speed)
        {
            Model.Speed.Value = speed;
        }

        protected void UpdateDirection(Vector2 input)
        {
            Model.TargetPosition.Value = input;
            const float minSqrMagnitude = 0.01f;
            Model.IsMoving.Value = input.normalized.sqrMagnitude > minSqrMagnitude;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            MovementStrategy?.Dispose();
        }
    }
}