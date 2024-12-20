using Interfaces.Core;
using UnityEngine;

namespace Core.StateMachine.States
{
    public class MoveToPositionState : State<IMovableEntityViewModel>
    {
        public MoveToPositionState(IMovableEntityViewModel context) : base(context)
        {
        }

        //TODO: Remove magic number
        public override void Update()
        {
            base.Update();

            if ((Context.TargetPosition.CurrentValue - (Vector2)Context.Transform.CurrentValue.position).sqrMagnitude <
                Context.DistanceThreshold * Context.DistanceThreshold ||
                (Context.IsMoving.CurrentValue == false && Context.IsMovingToDirection))
                ChangeState<IdleState>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Context.MovementStrategy.Move(Context.TargetPosition.CurrentValue);
        }
    }
}