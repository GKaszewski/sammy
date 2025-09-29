using Sammy.Models;
using UnityEngine;

namespace Sammy.Interfaces
{
    public interface IInputService
    {
        Vector2 MoveInput { get; }
        bool IsJumpPressed { get; }
        bool IsRunHeld { get; }
        bool IsFirePressed { get; }
        QuickTurnDirection QuickTurnInput { get; }
    }
}