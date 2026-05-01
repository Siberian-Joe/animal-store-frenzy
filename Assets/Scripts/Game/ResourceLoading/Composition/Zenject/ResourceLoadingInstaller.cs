using Modules.ResourceLoading.Runtime;
using Modules.ResourceLoading.Runtime.Contracts;
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