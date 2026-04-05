using Game.World.Core;

namespace Game.World.Composition
{
    public interface IEntityComposer
    {
        void Compose(EntityRoot root);
    }
}