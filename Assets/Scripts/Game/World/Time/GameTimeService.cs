using System;
using R3;
using Zenject;

namespace Game.World.GameTime
{
    public sealed class GameTimeService : IGameTimeReader, IGameTimeProgression, IInitializable, IDisposable
    {
        private readonly GameTimeConfig _config;
        private readonly IGameTimeStateStore _stateStore;
        private readonly Subject<GameTimeSnapshot> _minuteChanged = new();

        private GameTimeState _state;
        private double _realSecondsAccumulator;
        private float _realSecondsSinceSave;
        private bool _dirty;
        private bool _initialized;

        public GameTimeService(GameTimeConfig config, IGameTimeStateStore stateStore)
        {
            _config = config != false ? config : throw new ArgumentNullException(nameof(config));
            _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
            _state = new GameTimeState { TotalMinutes = _config.StartTotalMinutes };
        }

        public GameTimeSnapshot Current => new(_state.TotalMinutes);

        public Observable<GameTimeSnapshot> MinuteChanged => _minuteChanged;

        public void Initialize()
        {
            if (_initialized)
                return;

            _config.Validate();

            if (_stateStore.TryLoad(out var restoredState))
            {
                restoredState.TotalMinutes = Math.Max(0L, restoredState.TotalMinutes);
                _state = restoredState;
            }
            else
            {
                _state = new GameTimeState { TotalMinutes = _config.StartTotalMinutes };
            }

            _realSecondsAccumulator = 0d;
            _realSecondsSinceSave = 0f;
            _dirty = false;
            _initialized = true;
        }

        public void Advance(float realDeltaSeconds)
        {
            if (_initialized == false)
                throw new InvalidOperationException($"{nameof(GameTimeService)} must be initialized before advancing time.");

            if (realDeltaSeconds <= 0f)
                return;

            _realSecondsAccumulator += realDeltaSeconds;
            _realSecondsSinceSave += realDeltaSeconds;

            var secondsPerGameMinute = _config.RealSecondsPerGameMinute;
            var elapsedGameMinutes = (long)Math.Floor(_realSecondsAccumulator / secondsPerGameMinute);

            if (elapsedGameMinutes > 0L)
            {
                if (long.MaxValue - _state.TotalMinutes < elapsedGameMinutes)
                    throw new InvalidOperationException("Game time exceeded the supported minute range.");

                _realSecondsAccumulator -= elapsedGameMinutes * secondsPerGameMinute;

                for (var index = 0L; index < elapsedGameMinutes; index++)
                {
                    _state.TotalMinutes++;
                    _minuteChanged.OnNext(Current);
                }

                _dirty = true;
            }

            if (_dirty && _realSecondsSinceSave >= _config.SaveIntervalRealSeconds)
                Save();
        }

        public void Dispose()
        {
            if (_initialized && _dirty)
                Save();

            _minuteChanged.Dispose();
        }

        private void Save()
        {
            _stateStore.Save(_state);
            _dirty = false;
            _realSecondsSinceSave = 0f;
        }
    }
}
