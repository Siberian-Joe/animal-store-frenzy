namespace Game.World.Store
{
    public interface ICustomerCycleProgress
    {
        CustomerCycleStage CurrentStage { get; }

        bool IsAtLeast(CustomerCycleStage stage);
    }
}
