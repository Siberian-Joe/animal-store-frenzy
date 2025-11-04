using System;
using NewCore.Data;
using NewCore.Views;
using Object = UnityEngine.Object;

namespace NewCore.Services.EntityTypeRegistry
{
    public interface IEntityTypeRegistry
    {
        void RegisterType<TView>(
            Func<TView, IEntityProxy> proxyFactory,
            Action<GameState, IEntityProxy> addToGameState,
            Func<GameState, bool> shouldInitFromScene = null)
            where TView : Object, IView;

        void RegisterType<TView, TProxy>(
            Func<TView, TProxy> proxyFactory,
            Action<GameState, TProxy> addToGameState,
            Func<GameState, bool> shouldInitFromScene = null)
            where TView : Object, IView
            where TProxy : IEntityProxy;

        void ProcessSceneEntities(GameState gameState);
    }
}