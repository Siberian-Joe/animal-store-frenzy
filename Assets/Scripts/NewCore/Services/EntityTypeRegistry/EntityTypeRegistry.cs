using System;
using System.Collections.Generic;
using NewCore.Data;
using NewCore.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NewCore.Services.EntityTypeRegistry
{
    public class EntityTypeRegistry : IEntityTypeRegistry
    {
        private readonly List<EntityTypeHandler> _handlers = new();

        public void RegisterType<TView>(
            Func<TView, IEntityProxy> proxyFactory,
            Action<GameState, IEntityProxy> addToGameState,
            Func<GameState, bool> shouldInitFromScene)
            where TView : Object, IView
        {
            if (shouldInitFromScene == null)
                throw new ArgumentNullException(nameof(shouldInitFromScene));

            _handlers.Add(new EntityTypeHandler
            {
                ViewType = typeof(TView),
                ProxyFactory = view => proxyFactory((TView)view),
                AddToGameState = addToGameState,
                ShouldInit = shouldInitFromScene
            });
        }

        public void RegisterType<TView, TProxy>(
            Func<TView, TProxy> proxyFactory,
            Action<GameState, TProxy> addToGameState,
            Func<GameState, bool> shouldInitFromScene)
            where TView : Object, IView where TProxy : IEntityProxy
        {
            RegisterType<TView>(
                view => proxyFactory(view),
                (gameState, proxy) => addToGameState(gameState, (TProxy)proxy),
                shouldInitFromScene
            );
        }

        public void ProcessSceneEntities(GameState gameState)
        {
            foreach (var handler in _handlers)
                ProcessHandler(handler, gameState);
        }

        private static void ProcessHandler(EntityTypeHandler handler, GameState gameState)
        {
            if (handler.ShouldInit(gameState))
                InitializeProxiesFromScene(handler.ViewType, gameState, handler.ProxyFactory, handler.AddToGameState);
        }

        private static void InitializeProxiesFromScene(
            Type viewType,
            GameState gameState,
            Func<IView, IEntityProxy> proxyFactory,
            Action<GameState, IEntityProxy> addToGameState)
        {
            var views = Object.FindObjectsByType(
                viewType,
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            foreach (var raw in views)
            {
                if (raw is not IView view)
                    continue;

                var proxy = proxyFactory(view);
                addToGameState(gameState, proxy);
            }
        }

        private class EntityTypeHandler
        {
            public Type ViewType;
            public Func<IView, IEntityProxy> ProxyFactory;
            public Action<GameState, IEntityProxy> AddToGameState;
            public Func<GameState, bool> ShouldInit;
        }
    }
}