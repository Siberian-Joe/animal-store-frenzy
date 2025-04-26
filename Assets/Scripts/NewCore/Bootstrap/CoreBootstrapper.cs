using Cysharp.Threading.Tasks;
using NewCore.Commands;
using NewCore.Data;
using NewCore.Data.UI;
using NewCore.Extensions;
using NewCore.Services;
using NewCore.Services.Lifecycle;
using NewCore.Services.UI;
using NewCore.ViewModels.UI;
using NewCore.Views.UI;
using ObservableCollections;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NewCore.Bootstrap
{
    public sealed class CoreBootstrapper : Bootstrapper
    {
        private readonly IPanelService _panelService;
        private readonly ISceneLoader _sceneLoader;
        private readonly IGameDataService _gameDataService;
        private readonly ICustomerLifecycle _customerLifecycle;
        private readonly ICommandProcessor _commandProcessor;

        public CoreBootstrapper(IPanelService panelService, ISceneLoader sceneLoader, IGameDataService gameDataService,
            ICustomerLifecycle customerLifecycle, ICommandProcessor commandProcessor)
        {
            _panelService = panelService;
            _sceneLoader = sceneLoader;
            _gameDataService = gameDataService;
            _customerLifecycle = customerLifecycle;
            _commandProcessor = commandProcessor;
        }

        protected override async UniTask InitializeInternalAsync()
        {
            var coreScreen = await _panelService
                .LoadPanelAsync<CoreScreen, CoreScreenProxy, CoreScreenViewModel>()
                .AddTo(Disposables);

            coreScreen.Open();

            var gameState = await _gameDataService.LoadAsync<GameState, GameStateProxy>();

            _customerLifecycle.Initialize(gameState.Customers);
            _commandProcessor.RegisterHandler(new SpawnCustomerCommandHandler(_gameDataService));

            // TODO: This is only used to switch between scenes. Just a placeholder
            coreScreen.Context.Clicked
                .Subscribe(async _ => await _sceneLoader.LoadSceneAsync(SceneIdentifier.MainMenu))
                .AddTo(Disposables);

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
    }
}