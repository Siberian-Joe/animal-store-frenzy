using System;
using System.IO;
using UnityEngine;
using Zenject;

namespace Game.World.GameTime.Composition.Zenject
{
    public sealed class GameTimeInstaller : MonoInstaller
    {
        [SerializeField] private GameTimeConfig _config;
        [SerializeField] private string _saveFileName = "game-time.json";

        public override void InstallBindings()
        {
            if (_config == false)
                throw new InvalidOperationException($"{nameof(GameTimeInstaller)} requires assigned {nameof(GameTimeConfig)}.");

            if (string.IsNullOrWhiteSpace(_saveFileName))
                throw new InvalidOperationException($"{nameof(GameTimeInstaller)} requires a non-empty save file name.");

            _config.Validate();

            Container
                .BindInstance(_config)
                .WhenInjectedInto<GameTimeService>();

            Container
                .Bind<IGameTimeStateStore>()
                .To<JsonGameTimeStateStore>()
                .AsSingle()
                .WithArguments(Path.Combine(Application.persistentDataPath, _saveFileName.Trim()));

            Container
                .BindInterfacesAndSelfTo<GameTimeService>()
                .AsSingle();
        }
    }
}
