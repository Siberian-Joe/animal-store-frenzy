using UnityEngine;

namespace Game.World.PlayerInteraction
{
    public readonly struct PlayerWorldClick
    {
        public Vector2 ScreenPosition { get; }
        public Vector3 WorldPosition { get; }
        public Component HitComponent { get; }
        public bool HasHit { get; }

        public PlayerWorldClick(
            Vector2 screenPosition,
            Vector3 worldPosition,
            Component hitComponent,
            bool hasHit)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
            HitComponent = hitComponent;
            HasHit = hasHit;
        }
    }
}