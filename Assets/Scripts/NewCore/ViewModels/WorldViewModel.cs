using NewCore.Services.Lifecycle;
using R3;

namespace NewCore.ViewModels
{
    public class WorldViewModel : ViewModel
    {
        public readonly Observable<PlayerViewModel> Player;

        public WorldViewModel(IPlayerService playerService) => Player = playerService.Player;
    }
}