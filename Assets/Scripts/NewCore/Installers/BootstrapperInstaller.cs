using NewCore.Bootstrap;
using Zenject;

namespace NewCore.Installers
{
    public abstract class BootstrapperInstaller<T> : MonoInstaller where T : IAsyncSceneBootstrapper
    {
        public override void InstallBindings() => Container.BindInterfacesAndSelfTo<T>().AsSingle();
    }
}