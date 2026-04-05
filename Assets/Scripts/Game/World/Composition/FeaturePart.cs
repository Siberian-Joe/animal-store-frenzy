using Game.World.Core;
using R3;
using UnityEngine;

namespace Game.World.Composition
{
    public abstract class FeaturePart<TFeature> : MonoBehaviour, IFeaturePart<TFeature>
        where TFeature : class, IEntityFeature
    {
        public abstract void Bind(TFeature feature, CompositeDisposable disposables);
    }
}
