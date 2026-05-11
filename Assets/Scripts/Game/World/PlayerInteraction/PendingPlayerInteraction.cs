using System;
using Game.World.Interactions;

namespace Game.World.PlayerInteraction
{
    public sealed class PendingPlayerInteraction : IPendingPlayerInteraction
    {
        private InteractionOption _option;

        public bool HasValue => _option != null;

        public void Set(InteractionOption option) =>
            _option = option ?? throw new ArgumentNullException(nameof(option));

        public void Clear() => _option = null;

        public bool TryConsume(out InteractionOption option)
        {
            option = _option;
            _option = null;
            return option != null;
        }
    }
}