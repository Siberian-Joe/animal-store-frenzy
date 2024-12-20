using Interfaces.Core;
using UnityEngine;

namespace Core.StateMachine.States
{
    public class IdleState : State<IMovableEntityViewModel>
    {
        public IdleState(IMovableEntityViewModel context) : base(context)
        {
        }

        //TODO: Remove magic number
        public override void Update()
        {
            base.Update();

            if ((Context.IsMovingToDirection == false &&
                 (Context.TargetPosition.CurrentValue - (Vector2)Context.Transform.CurrentValue.position).sqrMagnitude >
                 Context.DistanceThreshold * Context.DistanceThreshold) || Context.IsMoving.CurrentValue)
                ChangeState<MoveToPositionState>();
        }
    }
}