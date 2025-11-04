using NewCore.Modules.Interaction.Abstractions;
using NewCore.ViewModels.World;

namespace NewCore.Views.World
{
    public class ShelfView : EntityView<ShelfViewModel>, IInteractable
    {
        public void Interact(IActor initiator) => ViewModel.Interact(initiator);
    }
}