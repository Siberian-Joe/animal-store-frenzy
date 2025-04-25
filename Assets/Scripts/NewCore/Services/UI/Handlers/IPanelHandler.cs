using NewCore.ViewModels;

namespace NewCore.Services.UI.Handlers
{
    public interface IPanelHandler : IPanel
    {
    }

    public interface IPanelHandler<out TViewModel> : IPanelHandler where TViewModel : IViewModel
    {
        TViewModel Context { get; }
    }
}