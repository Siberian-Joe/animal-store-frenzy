using Game.World.Interactions;

namespace Game.World.PlayerInteraction
{
    public interface IPendingPlayerInteraction
    {
        bool HasValue { get; }
        void Set(InteractionOption option);
        void Clear();
        bool TryConsume(out InteractionOption option);
    }
}