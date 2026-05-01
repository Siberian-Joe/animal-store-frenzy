namespace Modules.SceneNavigation.Runtime.Contracts
{
    public interface ISceneNavigator
    {
        bool IsTransitionRunning { get; }

        bool TryGoTo<TRoute>()
            where TRoute : SceneRoute;
    }
}