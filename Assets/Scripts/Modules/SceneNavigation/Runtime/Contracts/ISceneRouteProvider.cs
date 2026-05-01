using System;

namespace Modules.SceneNavigation.Runtime.Contracts
{
    public interface ISceneRouteProvider
    {
        Type RouteType { get; }
    }
}