using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public abstract class SingleActiveRegistry
    {
        protected IPanelHandler Active { get; private set; }

        protected void Register(IPanelHandler handler)
        {
            if (Active != null && Active != handler)
                Active.Close();

            Active = handler;
        }
    }
}