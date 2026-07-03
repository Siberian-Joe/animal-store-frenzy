namespace Game.World.Store
{
    public interface IStoreShiftRewardPolicy
    {
        int GetReward(StoreShiftRewardContext context);
    }
}