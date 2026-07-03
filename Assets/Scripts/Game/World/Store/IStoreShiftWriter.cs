namespace Game.World.Store
{
    public interface IStoreShiftWriter : IStoreShift
    {
        void StartShift();
        void MarkCustomerEntered();
        void MarkCustomerServed(int reward);
        void CompleteShift();
    }
}