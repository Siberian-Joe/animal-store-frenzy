using System;
using Zenject;

namespace Game.World.GameTime
{
    public sealed class GameTimeAdvancer : ITickable
    {
        private readonly IGameTimeProgression _progression;

        public GameTimeAdvancer(IGameTimeProgression progression) =>
            _progression = progression ?? throw new ArgumentNullException(nameof(progression));

        public void Tick() => _progression.Advance(UnityEngine.Time.deltaTime);
    }
}
