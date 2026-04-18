namespace Game.World.Shop.Customers
{
    public interface ICustomerProfile
    {
        string ArchetypeId { get; }

        void SetArchetype(string archetypeId);
    }
}