namespace Game.World.Interactions
{
    public interface IInteractionRoleResolver
    {
        bool TryResolve<TContract>(out TContract contract)
            where TContract : class, IInteractionContract;
    }
}