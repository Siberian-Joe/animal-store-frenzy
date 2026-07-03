using UnityEngine;

namespace Game.World.Store
{
    public sealed class FixedStoreShiftRewardPolicy : IStoreShiftRewardPolicy
    {
        private readonly int _rewardPerCustomer;

        public FixedStoreShiftRewardPolicy(int rewardPerCustomer) =>
            _rewardPerCustomer = Mathf.Max(0, rewardPerCustomer);

        public int GetReward(StoreShiftRewardContext context) => _rewardPerCustomer;
    }
}