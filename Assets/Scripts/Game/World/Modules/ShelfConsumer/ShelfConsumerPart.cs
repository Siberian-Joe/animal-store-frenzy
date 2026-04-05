using Game.World.Composition;
using R3;
using UnityEngine;

namespace Game.World.ShelfConsumer
{
    [DisallowMultipleComponent]
    public class ShelfConsumerPart : FeaturePart<IShelfConsumerFeature>
    {
        [SerializeField] [Min(1)] private int _transferAmount = 1;

        public int TransferAmount => _transferAmount;

        public override void Bind(IShelfConsumerFeature feature, CompositeDisposable disposables)
        {
        }
    }
}
