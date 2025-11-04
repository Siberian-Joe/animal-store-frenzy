using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Commands;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Domain.UI;
using NewCore.Extensions;
using NewCore.Factories;
using NewCore.Modules.Interaction;
using NewCore.Modules.Interaction.Commands;
using NewCore.Services.EntityTypeRegistry;
using NewCore.Services.GameData;
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
        private readonly IShelvesLifecycle _shelvesLifecycle;
        private readonly ICommandProcessor _commandProcessor;
        private readonly IPlayerService _playerService;
        private readonly ISceneNodeComposer _sceneNodeComposer;
        private readonly IEntityTypeRegistry _entityTypeRegistry;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IProxyFactory _proxyFactory;

        public CoreBootstrapper(
            IPanelService panelService,
            ISceneLoader sceneLoader,
            IGameDataService gameDataService,
            ICustomerLifecycle customerLifecycle,
            IShelvesLifecycle shelvesLifecycle,
            ICommandProcessor commandProcessor,
            IPlayerService playerService,
            ISceneNodeComposer sceneNodeComposer,
            IEntityTypeRegistry entityTypeRegistry,
            IViewModelFactory viewModelFactory,
            IProxyFactory proxyFactory)
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
            _proxyFactory = proxyFactory;
            _shelvesLifecycle = shelvesLifecycle;
        }

        protected override async UniTask InitializeInternalAsync(CancellationToken cancellationToken = default)
        {
            var coreScreen = await _panelService
                                   .LoadPanelAsync<CoreScreenView, CoreScreenModel, CoreScreen, CoreScreenViewModel>(
                                       cancellationToken)
                                   .AddTo(Disposables);

            coreScreen.Open();

            var result = await _gameDataService.LoadAsync<GameStateData, GameState>(cancellationToken);
            if (result.IsSuccess == false)
                return;

            var state = result.Value;

            _customerLifecycle.Initialize(state.Customers);
            _shelvesLifecycle.Initialize(state.Shelves);

            _commandProcessor.RegisterHandler(new InitializePlayerCommandHandler(_playerService, _viewModelFactory));
            _commandProcessor.RegisterHandler(new MovePlayerCommandHandler(_gameDataService));
            _commandProcessor.RegisterHandler(new SpawnCustomerCommandHandler(_gameDataService, _proxyFactory));
            _commandProcessor.RegisterHandler(new InteractionCommandHandler());

            // TODO: This is only used to switch between scenes. Just a placeholder
            coreScreen.Context.Clicked
                      .Subscribe(async _ =>
                                     await _sceneLoader.LoadSceneAsync(SceneIdentifier.MainMenu, cancellationToken))
                      .AddTo(Disposables);

            state.Customers.ObserveAdd()
                 .Subscribe(addEvent =>
                                Debug.Log($"Customer {addEvent.Value.Id} spawned at {addEvent.Value.Position}"));

            // TODO: Devise a more robust type registration approach and refactor this placeholder
            _entityTypeRegistry.RegisterType<PlayerView, Player>(
                view => new PlayerData
                {
                    Id = view.Id,
                    Position = view.transform.position
                }.ToProxy<Player>(_proxyFactory),
                (gameState, proxy) => gameState.Player.OnNext(proxy),
                gameState => gameState.Player.Value == null
            );

            _entityTypeRegistry.RegisterType<ShelfView, Shelf>(
                view => new ShelfData
                {
                    Id = view.Id,
                    Position = view.transform.position
                }.ToProxy<Shelf>(_proxyFactory),
                (gameState, proxy) => gameState.Shelves.Add(proxy)
            );

            _sceneNodeComposer.Initialize();
            _entityTypeRegistry.ProcessSceneEntities(state);
            _commandProcessor.TryProcess(new InitializePlayerCommand(state.Player.Value));

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