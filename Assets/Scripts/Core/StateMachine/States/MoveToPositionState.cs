using Interfaces.Core;
using UnityEngine;

namespace Core.StateMachine
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

            if ((Context.TargetPosition.Value - (Vector2)Context.Transform.Value.position).sqrMagnitude <
                Context.DistanceThreshold * Context.DistanceThreshold || (Context.IsMoving.Value == false && Context.IsMovingToDirection))
                ChangeState<IdleState>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Context.MovementStrategy.Move(Context.TargetPosition.Value);
        }
    }
}