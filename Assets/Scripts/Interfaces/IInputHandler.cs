using Sammy.Models;
using UnityEngine;

namespace Sammy.Interfaces
{
    public interface IInputHandler
    {
        Vector2 MoveInput { get; }
        bool JumpInputDown { get; }
        bool RunInput { get; }
        QuickTurnDirection QuickTurnInput { get; }
    }
}