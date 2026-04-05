using Game.World.Composition;
using R3;
using UnityEngine;

namespace Game.World.ProductContainer
{
    [DisallowMultipleComponent]
    public class ProductContainerPart : FeaturePart<IProductContainerFeature>
    {
        [SerializeField] [Min(0)] private int _capacity = 10;
        [SerializeField] [Min(0)] private int _initialQuantity;

        public int Capacity => _capacity;
        public int InitialQuantity => Mathf.Clamp(_initialQuantity, 0, _capacity);

        public override void Bind(IProductContainerFeature feature, CompositeDisposable disposables)
        {
        }
    }
}
