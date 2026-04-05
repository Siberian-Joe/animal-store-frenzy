using Game.World.Core;
using R3;

namespace Game.World.Composition
{
    public interface IFeaturePart
    {
    }

    public interface IFeaturePart<in TFeature> : IFeaturePart
        where TFeature : class, IEntityFeature
    {
        void Bind(TFeature feature, CompositeDisposable disposables);
    }
}
