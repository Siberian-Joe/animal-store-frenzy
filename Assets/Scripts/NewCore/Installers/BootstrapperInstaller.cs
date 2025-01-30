using NewCore.Bootstrap;
using Zenject;

namespace NewCore.Installers
{
    public abstract class BootstrapperInstaller<T> : MonoInstaller where T : IBootstrapper
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<T>().AsSingle();
        }
    }
}