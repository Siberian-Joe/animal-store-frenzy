namespace Game.World.EntityRuntime
{
    public interface IEntityComponent
    {
        int ActivationOrder { get; }

        bool IsActive { get; }

        void Activate();

        void Deactivate();
    }
}
