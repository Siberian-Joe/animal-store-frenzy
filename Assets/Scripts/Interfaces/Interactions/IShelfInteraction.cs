namespace Interfaces.Interactions
{
    public interface IShelfInteraction : IInteraction
    {
        void AddItem(int amount);
        void RemoveItem(int amount);
    }
}