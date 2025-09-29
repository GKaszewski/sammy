using System;
using KBCore.Refs;
using UnityEngine;

namespace Sammy
{
    public class PlayerStamina : MonoBehaviour
    {
        [SerializeField, Self] private PlayerInput playerInput;
        
        [Header("Stamina Settings")]
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaDrainRate = 5f;
        
        private float _currentStamina;
        
        public bool CanRun { get; private set; }

        private void Start()
        {
            _currentStamina = maxStamina;
        }

        private void Update()
        {
            CanRun = playerInput.RunInput && _currentStamina > 0f && playerInput.MoveInput.y > 0f;

            if (CanRun)
            {
                _currentStamina -= staminaDrainRate * Time.deltaTime;
            }
        }
    }
}