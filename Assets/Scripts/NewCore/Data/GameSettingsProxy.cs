using System;
using R3;

namespace NewCore.Data
{
    [Serializable]
    public class GameSettingsProxy : Proxy<GameSettingsState>
    {
        public ReactiveProperty<int> MusicVolume { get; private set; }
        public ReactiveProperty<int> SfxVolume { get; private set; }

        public override void Initialize(GameSettingsState data)
        {
            MusicVolume = new ReactiveProperty<int>(data.MusicVolume);
            SfxVolume = new ReactiveProperty<int>(data.SfxVolume);

            MusicVolume
                .Subscribe(value => data.MusicVolume = value)
                .AddTo(Disposables);

            SfxVolume
                .Subscribe(value => data.SfxVolume = value)
                .AddTo(Disposables);
        }

        public override GameSettingsState ToModel()
        {
            return new GameSettingsState
            {
                MusicVolume = MusicVolume.Value,
                SfxVolume = SfxVolume.Value
            };
        }

        public override void Dispose()
        {
            base.Dispose();

            MusicVolume.Dispose();
            SfxVolume.Dispose();
        }
    }
}