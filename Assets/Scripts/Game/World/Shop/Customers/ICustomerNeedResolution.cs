using Game.World.EntityRuntime;

namespace Game.World.Shop.Customers
{
    public interface ICustomerNeedResolution
    {
        bool HasPendingShelfVisit(EntityRoot customerRoot, ICustomerNeeds customerNeeds);

        int AbandonUnresolvableNeeds(EntityRoot customerRoot, ICustomerNeeds customerNeeds);
    }
}
