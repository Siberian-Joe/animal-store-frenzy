namespace Game.World.Store
{
    public interface IStoreStatusWriter : IStoreStatus
    {
        void Open();
        void Close();
    }
}