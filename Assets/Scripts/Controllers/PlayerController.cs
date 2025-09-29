using Sammy.Interfaces;
using Sammy.Models;
using UnityEngine;
using VContainer.Unity;

namespace Sammy.Controllers
{
    public class PlayerController : ITickable
    {
        private readonly IInputService _inputService;
        private readonly IPlayerDataService _playerData;
        private readonly IPlayerView _playerView;
        private readonly IEffectsService _effectsService;
        private readonly IAudioService _audioService;

        private readonly float _walkSpeed = 5f; 
        private readonly float _runSpeed = 7f; 
        private readonly float _jumpForce = 5f; 
        private readonly float _gravity = 10f; 
        private Vector3 _velocity;
        
        public PlayerController(IInputService inputService, IPlayerDataService playerData, IPlayerView playerView, IEffectsService effectsService, IAudioService audioService)
        {
            _inputService = inputService;
            _playerData = playerData;
            _playerView = playerView;
            _effectsService = effectsService;
            _audioService = audioService;
        }
        
        public void Tick()
        {
            var isGrounded = _playerView.IsGrounded;
            
            if (isGrounded && _velocity.y < 0)
            {
                _velocity.y = -0.5f; 
            }
            else
            {
                _velocity.y -= _gravity * Time.deltaTime; 
            }

            var currentSpeed = _inputService.IsRunHeld ? _runSpeed : _walkSpeed; 
            var moveIntent = _playerView.Transform.forward * (_inputService.MoveInput.y * currentSpeed);
            _velocity.x = moveIntent.x;
            _velocity.z = moveIntent.z;

            if (_inputService.IsJumpPressed && isGrounded)
            {
                _velocity.y = _jumpForce; 
            }

            _playerView.Move(_velocity * Time.deltaTime);

            if (_inputService.MoveInput.magnitude > 0)
            {
                _playerView.Rotate(_inputService.MoveInput.x); 
            }

            if (_inputService.IsFirePressed && _playerData.Wasps.Value > 0)
            {
                _playerView.SpawnWasp(_playerData.CurrentWaspType); 
                _playerData.DecreaseWasps(); 
            }
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