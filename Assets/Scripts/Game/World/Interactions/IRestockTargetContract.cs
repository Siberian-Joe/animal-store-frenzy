namespace Game.World.Interactions
{
    public interface IRestockTargetContract : IInteractionContract
    {
        int FreeSpace { get; }

        bool IsFull { get; }

        int AddUpTo(int amount);
    }
}