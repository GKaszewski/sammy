using Sammy.Handlers;
using Sammy.Interfaces;
using Sammy.Models;
using UnityEngine;
using VContainer.Unity;

namespace Sammy.Controllers
{
    public class PlayerController : ITickable
    {
        private readonly IPlayerDataService _playerData;
        private readonly IPlayerView _playerView;
        private readonly IEffectsService _effectsService;
        private readonly IAudioService _audioService;
        
        private readonly PlayerMovementHandler _movementHandler;
        private readonly PlayerCombatHandler _combatHandler;
        
        public PlayerController(IPlayerDataService playerData, IPlayerView playerView, IEffectsService effectsService, IAudioService audioService, PlayerMovementHandler movementHandler, PlayerCombatHandler combatHandler)
        {
            _playerData = playerData;
            _playerView = playerView;
            _effectsService = effectsService;
            _audioService = audioService;
            _movementHandler = movementHandler;
            _combatHandler = combatHandler;
        }
        
        public void Tick()
        {
            _movementHandler.Tick();
            _combatHandler.Tick();
        }
        
        public void OnCrystalCollision(CrystalColor newCrystalColor)
        {
            _audioService.Play(AudioSounds.CrystalPickup);
            _effectsService.SpawnEffect(EffectType.POOF, _playerView.Transform.position);

            var currentColor = _playerData.CurrentCrystal.Value;
            if (currentColor != CrystalColor.NONE && currentColor != CrystalColor.MULTI)
            {
                // Here you'd inject an ICrystalService to handle mixing
                // For now, let's keep it simple:
                if (currentColor != newCrystalColor)
                {
                    _playerView.SpawnDroppedCrystal(currentColor);
                    _playerData.ChangeCrystal(newCrystalColor);
                }
            }
            else if (currentColor == CrystalColor.NONE)
            {
                _playerData.ChangeCrystal(newCrystalColor); 
            }
        }
    }
}