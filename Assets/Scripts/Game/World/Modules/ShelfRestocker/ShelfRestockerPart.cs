using Game.World.Composition;
using R3;
using UnityEngine;

namespace Game.World.ShelfRestocker
{
    [DisallowMultipleComponent]
    public class ShelfRestockerPart : FeaturePart<IShelfRestockerFeature>
    {
        [SerializeField]
        [Min(1)]
        private int _transferAmount = 1;

        public int TransferAmount => _transferAmount;

        public override void Bind(IShelfRestockerFeature feature, CompositeDisposable disposables)
        {
        }
    }
}
