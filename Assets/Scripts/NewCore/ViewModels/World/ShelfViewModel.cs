using NewCore.Commands;
using NewCore.Data;
using NewCore.Modules.Interaction.Abstractions;
using NewCore.Modules.Interaction.Commands;
using NewCore.Modules.Interaction.Context;

namespace NewCore.ViewModels.World
{
    public class ShelfViewModel : EntityViewModel<Shelf>, IInteractable
    {
        private readonly ICommandProcessor _processor;

        public ShelfViewModel(Shelf proxy, ICommandProcessor processor) : base(proxy) => _processor = processor;

        public void Interact(IActor initiator) =>
            _processor.TryProcess(new InteractionCommand(
                                      new InteractionContext(initiator, Proxy)));
    }
}