using NewCore.Data;
using NewCore.Modules.Interaction.Abstractions;
using NewCore.Views;
using R3;

namespace NewCore.ViewModels.World
{
    public class PlayerViewModel : NavigableEntityViewModel<Player>, INavigableEntityViewModel
    {
        public ReactiveCommand<IInteractable> Interact { get; }

        public PlayerViewModel(Player proxy) : base(proxy)
        {
            Interact = new ReactiveCommand<IInteractable>();
            Interact
                .Subscribe(target => target.Interact(Proxy))
                .AddTo(Disposables);
        }
    }
}