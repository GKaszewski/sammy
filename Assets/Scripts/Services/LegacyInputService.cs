using Sammy.Interfaces;
using Sammy.Models;
using UnityEngine;
using VContainer.Unity;

namespace Sammy.Services
{
    public class LegacyInputService : IInputService, ITickable
    {
        public Vector2 MoveInput { get; private set; }
        public bool IsJumpPressed { get; private set; }
        public bool IsRunHeld { get; private set; }
        public bool IsFirePressed { get; private set; }
        public QuickTurnDirection QuickTurnInput { get; private set; }
        
        private QuickTurnDirection GetQuickTurnDirection()
        {
            if (!Input.GetButtonDown("Quick turn")) return QuickTurnDirection.None;
            
            var horizontal = MoveInput.x;
            return (Mathf.Approximately(horizontal, 0)) ? QuickTurnDirection.Right : (horizontal > 0 ? QuickTurnDirection.Right : QuickTurnDirection.Left);
        }

        public void Tick()
        {
            MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            IsJumpPressed = Input.GetButtonDown("Jump");
            IsRunHeld = Input.GetButton("Run");
            IsFirePressed = Input.GetButtonDown("Fire1");
            QuickTurnInput = GetQuickTurnDirection();
        }
    }
}