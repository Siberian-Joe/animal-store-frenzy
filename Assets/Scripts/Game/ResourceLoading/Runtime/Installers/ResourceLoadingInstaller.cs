using Game.ResourceLoading.Contracts;
using Zenject;

namespace Game.ResourceLoading.Runtime.Installers
{
    public sealed class ResourceLoadingInstaller : MonoInstaller
    {
        public override void InstallBindings() =>
            Container
                .Bind<IResourceLoader>()
                .To<ResourceLoader>()
                .AsSingle();
    }
}