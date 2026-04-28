using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface IEntityReconstructionFactory
    {
        EntityRoot Reconstruct(EntityState state);

        void Destroy(EntityRoot root);
    }
}