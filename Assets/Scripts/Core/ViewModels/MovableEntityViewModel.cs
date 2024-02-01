using System.Collections.Generic;
using Core.Enums;
using Core.StateMachine;
using Interfaces.Core;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using UniRx;
using UnityEngine;

namespace Core.ViewModels
{
    public abstract class MovableEntityViewModel<TModel> : EntityViewModel<TModel>, IMovableEntityViewModel
        where TModel : MovableEntityModel
    {
        public IReadOnlyReactiveProperty<float> Speed => Model.Speed;
        public IReadOnlyReactiveProperty<Vector2> Direction => Model.Direction;
        public IReadOnlyReactiveProperty<Vector2> TargetPosition => Model.TargetPosition;
        public IReadOnlyReactiveProperty<bool> IsMoving => Model.IsMoving;
        public IMovementStrategy MovementStrategy { get; private set; }
        public float DistanceThreshold { get; protected set; } = 0.45f;
        public bool IsMovingToDirection { get; protected set; } = false;

        private readonly IStateMachine _stateMachine;

        //TODO: Figure out how to implement assigning a different set of states to each entity type
        protected MovableEntityViewModel(IDataService dataService, ILoggingService loggingService) : base(dataService)
        {
            var states = new List<IState>
            {
                new IdleState(this),
                new MoveToPositionState(this)
            };

            _stateMachine = new StateMachine.StateMachine(loggingService, states);
            _stateMachine.ChangeState<IdleState>();
        }
        
        public override void Update()
        {
            base.Update();
            
            _stateMachine.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            _stateMachine.FixedUpdate();
        }

        public void SetMovementStrategy(IMovementStrategy movementStrategy)
        {
            MovementStrategy = movementStrategy;
        }

        public void SetSpeed(float speed)
        {
            Model.Speed.Value = speed;
        }

        public void UpdateDirection(Vector2 input)
        {
            Model.Direction.Value = input;
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