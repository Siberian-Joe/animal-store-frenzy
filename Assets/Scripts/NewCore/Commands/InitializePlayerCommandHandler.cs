using NewCore.Data;
using NewCore.Factories;
using NewCore.Services.Lifecycle;
using NewCore.ViewModels;
using NewCore.ViewModels.World;

namespace NewCore.Commands
{
    public class InitializePlayerCommandHandler : ICommandHandler<InitializePlayerCommand>
    {
        private readonly IPlayerService _playerService;
        private readonly IViewModelFactory _viewModelFactory;

        public InitializePlayerCommandHandler(
            IPlayerService playerService,
            IViewModelFactory viewModelFactory)
        {
            _playerService = playerService;
            _viewModelFactory = viewModelFactory;
        }

        public bool Handle(InitializePlayerCommand command)
        {
            var viewModel = _viewModelFactory.Create<Player, PlayerViewModel>(command.Player);
            _playerService.Player.Value = viewModel;
            return true;
        }
    }
}