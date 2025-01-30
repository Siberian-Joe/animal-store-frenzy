using System;
using R3;

namespace NewCore.Data
{
    [Serializable]
    public class GameSettingsProxy : Proxy
    {
        public ReactiveProperty<int> MusicVolume { get; }
        public ReactiveProperty<int> SfxVolume { get; }

        public GameSettingsProxy(GameSettingsState gameSettingsState)
        {
            MusicVolume = new ReactiveProperty<int>(gameSettingsState.MusicVolume);
            SfxVolume = new ReactiveProperty<int>(gameSettingsState.SfxVolume);

            MusicVolume
                .Subscribe(value => gameSettingsState.MusicVolume = value)
                .AddTo(Disposables);

            SfxVolume
                .Subscribe(value => gameSettingsState.SfxVolume = value)
                .AddTo(Disposables);
        }

        public override void Dispose()
        {
            base.Dispose();

            MusicVolume.Dispose();
            SfxVolume.Dispose();
        }
    }
}