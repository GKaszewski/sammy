using Sammy.Interfaces;
using UnityEngine;
using VContainer.Unity;

namespace Sammy.Handlers
{
    public class PlayerMovementHandler : ITickable, IPlayerMovementHandler
    {
        private readonly IInputService _inputService;
        private readonly IPlayerView _playerView;
    
        // Movement Parameters
        private readonly float _walkSpeed = 5f;
        private readonly float _runSpeed = 7f;
        private readonly float _jumpForce = 5f;
        private readonly float _gravity = 10f;
        private Vector3 _velocity;
        
        public PlayerMovementHandler(IInputService inputService, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerView = playerView;
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
        }
    }
}