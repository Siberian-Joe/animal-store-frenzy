namespace Game.World.Interactions
{
    public interface ITakeSourceContract : IInteractionContract
    {
        int AvailableQuantity { get; }

        bool IsEmpty { get; }

        int RemoveUpTo(int amount);

        void ReturnUpTo(int amount);
    }
}