namespace Game.World.Store
{
    public interface ICustomerCycleProgressWriter : ICustomerCycleProgress
    {
        void Mark(CustomerCycleStage stage);
    }
}
