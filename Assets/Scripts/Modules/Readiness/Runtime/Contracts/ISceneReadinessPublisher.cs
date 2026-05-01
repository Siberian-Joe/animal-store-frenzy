using System;
using UnityEngine.SceneManagement;

namespace Modules.Readiness.Runtime.Contracts
{
    public interface ISceneReadinessPublisher
    {
        IDisposable Register(Scene scene, IReadiness readiness);
    }
}