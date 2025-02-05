using Cysharp.Threading.Tasks;
using NewCore.Views.UI;

namespace NewCore.Services.UI
{
    public interface IUIRootLoader
    {
        UniTask<UIRoot> GetUIRootAsync();
    }
}