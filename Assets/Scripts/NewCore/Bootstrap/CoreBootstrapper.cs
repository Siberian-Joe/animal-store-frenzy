using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Commands;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Extensions;
using NewCore.Factories;
using NewCore.Services;
using NewCore.Services.EntityTypeRegistry;
using NewCore.Services.Lifecycle;
using NewCore.Services.Scenes;
using NewCore.Services.UI;
using NewCore.ViewBinders;
using NewCore.ViewModels.UI;
using NewCore.Views.UI;
using NewCore.Views.World;
using ObservableCollections;
using R3;
using UnityEngine;
using CoreScreen = NewCore.Data.UI.CoreScreen;
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
        private readonly IPlayerService _playerService;
        private readonly ISceneNodeComposer _sceneNodeComposer;
        private readonly IEntityTypeRegistry _entityTypeRegistry;
        private readonly IViewModelFactory _viewModelFactory;

        public CoreBootstrapper(
            IPanelService panelService,
            ISceneLoader sceneLoader,
            IGameDataService gameDataService,
            ICustomerLifecycle customerLifecycle,
            ICommandProcessor commandProcessor,
            IPlayerService playerService,
            ISceneNodeComposer sceneNodeComposer,
            IEntityTypeRegistry entityTypeRegistry,
            IViewModelFactory viewModelFactory)
        {
            _panelService = panelService;
            _sceneLoader = sceneLoader;
            _gameDataService = gameDataService;
            _customerLifecycle = customerLifecycle;
            _commandProcessor = commandProcessor;
            _playerService = playerService;
            _sceneNodeComposer = sceneNodeComposer;
            _entityTypeRegistry = entityTypeRegistry;
            _viewModelFactory = viewModelFactory;
        }

        protected override async UniTask InitializeInternalAsync(CancellationToken cancellationToken = default)
        {
            var coreScreen = await _panelService
                .LoadPanelAsync<CoreScreenView, CoreScreen, CoreScreenViewModel>(cancellationToken)
                .AddTo(Disposables);

            coreScreen.Open();

            var result = await _gameDataService.LoadAsync<GameStateData, GameState>(cancellationToken);
            if (!result.IsSuccess)
                return;

            var state = result.Value;

            _customerLifecycle.Initialize(state.Customers);
            _commandProcessor.RegisterHandler(new InitializePlayerCommandHandler(_playerService, _viewModelFactory));
            _commandProcessor.RegisterHandler(new SpawnCustomerCommandHandler(_gameDataService));

            // TODO: This is only used to switch between scenes. Just a placeholder
            coreScreen.Context.Clicked
                .Subscribe(async _ => await _sceneLoader.LoadSceneAsync(SceneIdentifier.MainMenu, cancellationToken))
                .AddTo(Disposables);

            state.Customers.ObserveAdd().Subscribe(addEvent =>
                Debug.Log($"Customer {addEvent.Value.ID} spawned at {addEvent.Value.Position}"));

            // TODO: Devise a more robust type registration approach and refactor this placeholder
            _entityTypeRegistry.RegisterType<PlayerView, Player>(
                view => new PlayerData
                {
                    ID = view.ID,
                    Position = view.transform.position
                }.ToProxy<PlayerData, Player>(),
                (gameState, proxy) => gameState.Player.OnNext(proxy),
                gameState => gameState.Player.Value == null
            );

            _sceneNodeComposer.Initialize();
            _entityTypeRegistry.ProcessSceneEntities(state);
            _commandProcessor.TryProcess(new InitializePlayerCommand(state.Player.Value));

            // var worldViewModel = _viewModelFactory.Create<WorldViewModel>();
            // Object.FindAnyObjectByType<WorldView>().Bind(worldViewModel);
            //
            // var customersWorldViewModel = _viewModelFactory.Create<CustomersWorldViewModel>();
            // Object.FindAnyObjectByType<CustomersWorldView>().Bind(customersWorldViewModel);

            _customerLifecycle.TrySpawnCustomer("FirstCustomer",
                new Vector2(Random.Range(0, 10), Random.Range(0, 10)));
            _customerLifecycle.TrySpawnCustomer("SecondCustomer",
                new Vector2(Random.Range(0, 10), Random.Range(0, 10)));
            _customerLifecycle.TrySpawnCustomer("ThirdCustomer",
                new Vector2(Random.Range(0, 10), Random.Range(0, 10)));
            _customerLifecycle.TrySpawnCustomer("FourthCustomer",
                new Vector2(Random.Range(0, 10), Random.Range(0, 10)));
        }
    }
}