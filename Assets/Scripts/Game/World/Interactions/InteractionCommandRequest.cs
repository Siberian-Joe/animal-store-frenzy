using System;
using Game.World.Commands;
using UnityEngine;

namespace Game.World.Interactions
{
    public sealed class InteractionCommandRequest
    {
        public InteractionCommandRequest(
            Vector3 approachPoint,
            IGameCommand command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
            ApproachPoint = approachPoint;
        }

        public Vector3 ApproachPoint { get; }

        public IGameCommand Command { get; }
    }
}
