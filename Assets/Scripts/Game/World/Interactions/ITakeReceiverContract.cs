namespace Game.World.Interactions
{
    public interface ITakeReceiverContract : IInteractionContract
    {
        int TransferAmount { get; }

        int FreeSpace { get; }

        bool IsFull { get; }

        int AddUpTo(int amount);
    }
}