using System;
using R3;

namespace NewCore.Data
{
    [Serializable]
    public class GameSettings : Proxy<GameSettingsState>
    {
        public ReactiveProperty<int> MusicVolume { get; }
        public ReactiveProperty<int> SfxVolume { get; }

        public GameSettings(GameSettingsState model) : base(model)
        {
            MusicVolume = new ReactiveProperty<int>(model.MusicVolume);
            SfxVolume = new ReactiveProperty<int>(model.SfxVolume);

            MusicVolume
                .Subscribe(value => model.MusicVolume = value)
                .AddTo(Disposables);

            SfxVolume
                .Subscribe(value => model.SfxVolume = value)
                .AddTo(Disposables);
        }

        protected override GameSettingsState CreateModel()
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