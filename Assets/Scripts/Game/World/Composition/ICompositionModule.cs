namespace Game.World.Composition
{
    public interface ICompositionModule
    {
        int Order { get; }

        void Compose(EntityCompositionContext context);
    }
}
