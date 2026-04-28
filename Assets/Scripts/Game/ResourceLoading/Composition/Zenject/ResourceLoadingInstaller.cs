using Modules.ResourceLoading.Contracts;
using Modules.ResourceLoading.Runtime;
using Zenject;

namespace Game.ResourceLoading.Composition.Zenject
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