using Sammy.Interfaces;
using Sammy.Models;
using UnityEngine;

namespace Sammy
{
    // Notice: This class will be rewritten to utilize the new Input System in the future.
    // For legacy reasons, it still uses the old Input System.
    public class PlayerInput : MonoBehaviour, IInputService
    {
        public Vector2 MoveInput { get; private set; }
        public bool IsJumpPressed { get; private set; }
        public bool IsRunHeld { get; private set; }
        public bool IsFirePressed { get; }
        public QuickTurnDirection QuickTurnInput { get; private set; }

        private void Update()
        {
            MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            IsJumpPressed = Input.GetButtonDown("Jump");
            IsRunHeld = Input.GetButton("Run");
            
            if (Input.GetButtonDown("Quick turn"))
            {
                var horizontal = MoveInput.x;
                QuickTurnInput = (Mathf.Approximately(horizontal, 0)) ? QuickTurnDirection.Right : (horizontal > 0 ? QuickTurnDirection.Right : QuickTurnDirection.Left);
            }
            else
            {
                QuickTurnInput = QuickTurnDirection.None;
            }
        }
    }
}