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
                 (Context.TargetPosition.Value - (Vector2)Context.Transform.Value.position).sqrMagnitude >
                 Context.DistanceThreshold * Context.DistanceThreshold) || Context.IsMoving.Value)
                ChangeState<MoveToPositionState>();
        }
    }
}