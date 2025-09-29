using System;
using System.Threading.Tasks;
using KBCore.Refs;
using UnityEngine;

namespace Sammy
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField, Self] private CharacterController cc;
        [SerializeField, Self] private PlayerInput playerInput;
        [SerializeField, Self] private PlayerStamina playerStamina;
        
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = 10f;
        [SerializeField] private float knockbackTime = 0.15f;
        [SerializeField] private float rotationSpeed = 5f;
    
        [Header("Ground Check")]
        [SerializeField] private Transform feet;
        [SerializeField] private float groundDetectionRadius = 0.1f;
        [SerializeField] private LayerMask groundDetectionLayerMask;
        
        [Header("Dependencies")]
        [SerializeField] private Transform cameraParent;
        
        private Vector3 _velocity;
        private bool _isKnocked = false;
        private Vector3 _camF;
        private Vector3 _camR;
        
        public bool IsGrounded { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsRunning { get; private set; }
        public Vector3 Intent { get; private set; }
        public float RotationSpeed => rotationSpeed;

        private void Update()
        {
            UpdateCameraVectors();
            CheckGround();
            
            IsRunning = playerStamina.CanRun;
            
            if (!_isKnocked)
            {
                CalculateMovement();
            }
            
            ApplyGravity();
            HandleJumping();
            
            cc.Move(_velocity * Time.deltaTime);
            
            if (playerInput.MoveInput.magnitude > 0 && !IsJumping) transform.Rotate(Vector3.up * (playerInput.MoveInput.x * rotationSpeed * Time.deltaTime));

        }
        
        private void CheckGround()
        {
            IsGrounded = Physics.Raycast(feet.position, Vector3.down, groundDetectionRadius, groundDetectionLayerMask);
        }
        
        private void ApplyGravity()
        {
            if (IsGrounded && _velocity.y < 0)
            {
                _velocity.y = -0.5f;
            }
            else
            {
                _velocity.y -= gravity * Time.deltaTime;
            }
        }

        private void HandleJumping()
        {
            IsJumping = _velocity.y > 0 && !IsGrounded;
        
            if (playerInput.IsJumpPressed && IsGrounded)
            {
                _velocity.y = jumpForce;
            }
        }
    
        public async Task Knockback(Vector3 direction)
        {
            _isKnocked = true;
            _velocity = direction;
            await Task.Delay(TimeSpan.FromSeconds(knockbackTime));
            _isKnocked = false;
        }
        
        private void UpdateCameraVectors()
        {
            _camF = cameraParent.forward;
            _camR = cameraParent.right;
            _camF.y = 0;
            _camR.y = 0;
            _camF = _camF.normalized;
            _camR = _camR.normalized;
        }

        private void CalculateMovement()
        {
            var currentSpeed = IsRunning ? runSpeed : walkSpeed;
            var velocityXZ = transform.forward * (playerInput.MoveInput.y * currentSpeed);
            _velocity = new Vector3(velocityXZ.x, _velocity.y, velocityXZ.z);
            Intent = transform.forward * playerInput.MoveInput.y;
        }
    }
}