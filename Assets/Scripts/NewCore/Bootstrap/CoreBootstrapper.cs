using System;
using NewCore.Commands;
using NewCore.Data;
using NewCore.Installers;
using NewCore.Services;
using NewCore.Services.Lifecycle;
using NewCore.Services.UI;
using ObservableCollections;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NewCore.Bootstrap
{
    public sealed class CoreBootstrapper : IBootstrapper
    {
        private readonly IUIRootLoader _uiRootLoader;
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameDataService _gameDataService;
        private readonly ICustomerLifecycle _customerLifecycle;
        private readonly ICommandProcessor _commandProcessor;
        private readonly CompositeDisposable _disposables = new();

        public CoreBootstrapper(IUIRootLoader uiRootLoader, ISceneLoader sceneLoader, IGameDataService gameDataService,
            ICustomerLifecycle customerLifecycle, ICommandProcessor commandProcessor)
        {
            _uiRootLoader = uiRootLoader;
            _sceneLoader = sceneLoader;
            _gameDataService = gameDataService;
            _customerLifecycle = customerLifecycle;
            _commandProcessor = commandProcessor;
        }

        public async void Initialize()
        {
            try
            {
                var uiRoot = await _uiRootLoader.GetUIRootAsync();
                var mainMenu = uiRoot.EnableCore();
                var gameState = await _gameDataService.LoadAsync<GameState, GameStateProxy>();

                _customerLifecycle.Initialize(gameState.Customers);
                _commandProcessor.RegisterHandler(new SpawnCustomerCommandHandler(_gameDataService));

                // TODO: This is only used to switch between scenes. Just a placeholder
                mainMenu.Clicked
                    .Subscribe(async _ => await _sceneLoader.LoadSceneAsync(SceneIdentifier.MainMenu))
                    .AddTo(_disposables);

                gameState.Customers.ObserveAdd().Subscribe(change =>
                    Debug.Log($"Customer {change.Value.Id} spawned at {change.Value.Position}"));

                await _customerLifecycle.TrySpawnCustomer("FirstCustomer",
                    new Vector3Int(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)));
                await _customerLifecycle.TrySpawnCustomer("SecondCustomer",
                    new Vector3Int(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)));
                await _customerLifecycle.TrySpawnCustomer("ThirdCustomer",
                    new Vector3Int(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)));
                await _customerLifecycle.TrySpawnCustomer("FourthCustomer",
                    new Vector3Int(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)));
            }
            catch (Exception exception)
            {
                Debug.LogError("Failed to initialize core: " + exception.Message);
            }
        }

        public void Dispose() => _disposables.Dispose();
    }
}