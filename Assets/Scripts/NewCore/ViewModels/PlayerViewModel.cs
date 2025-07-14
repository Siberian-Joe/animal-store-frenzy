using NewCore.Data;
using NewCore.Views;

namespace NewCore.ViewModels
{
    public class PlayerViewModel : NavigableEntityViewModel<Player>, INavigableEntityViewModel
    {
        public PlayerViewModel(Player proxy) : base(proxy)
        {
        }
    }
}