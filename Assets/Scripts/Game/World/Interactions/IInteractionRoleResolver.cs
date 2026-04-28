namespace Game.World.Interactions
{
    public interface IInteractionActor
    {
        bool TryGetRole<TRole>(out TRole role)
            where TRole : class, IInteractionRole;
    }
}