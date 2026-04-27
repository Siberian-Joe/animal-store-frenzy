namespace Game.Presentation.Runtime.Lifecycle
{
    public interface IPanelLifecycle
    {
        void Opened();
        void Closed();
        void Released();
    }
}