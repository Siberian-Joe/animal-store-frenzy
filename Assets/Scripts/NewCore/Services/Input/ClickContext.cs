using NewCore.Modules.Interaction.Abstractions;
using UnityEngine;

namespace NewCore.Services.Input
{
    public readonly struct ClickContext
    {
        public Vector2 WorldPosition { get; }
        public Collider2D HitCollider { get; }
        public IInteractable Interactable { get; }
        public bool IsInteractable => Interactable != null;

        public ClickContext(Vector2 worldPosition, Collider2D collider)
        {
            WorldPosition = worldPosition;
            HitCollider = collider;
            Interactable = collider?.GetComponent<IInteractable>();
        }
    }
}