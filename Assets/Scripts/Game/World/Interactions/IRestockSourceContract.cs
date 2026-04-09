namespace Game.World.Interactions
{
    public interface IRestockSourceContract : IInteractionContract
    {
        int TransferAmount { get; }

        int AvailableQuantity { get; }

        int RemoveUpTo(int amount);

        void ReturnUpTo(int amount);
    }
}
