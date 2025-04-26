using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public abstract class SingleActiveRegistry : ISingleActiveRegistry
    {
        public IPanelHandler Active { get; private set; }

        public void Register(IPanelHandler handler)
        {
            if (Active != null && Active != handler)
                Active.Close();

            Active = handler;
        }

        public void Unregister(IPanelHandler handler)
        {
            if (Active == handler)
                Active = null;
        }
    }
}