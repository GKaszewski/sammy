using Sammy.Interfaces;
using VContainer.Unity;

namespace Sammy.Handlers
{
    public class PlayerCombatHandler : ITickable, IPlayerCombatHandler
    {
        private readonly IInputService _inputService;
        private readonly IPlayerDataService _playerData;
        private readonly IPlayerView _playerView;

        public PlayerCombatHandler(IInputService inputService, IPlayerDataService playerData, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerData = playerData;
            _playerView = playerView;
        }
        
        public void Tick()
        {
            if (_inputService.IsFirePressed && _playerData.Wasps.Value > 0)
            {
                _playerView.SpawnWasp(_playerData.CurrentWaspType);
                _playerData.DecreaseWasps();
            }
        }
    }
}