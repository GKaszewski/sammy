using UnityEngine;

namespace Sammy.Interfaces
{
    public interface IPlayerView
    {
        Transform Transform { get; }
        bool IsGrounded { get; }
        void Move(Vector3 velocity);
        void Rotate(float direction);
        void ApplyJumpForce();
        void SpawnWasp(WaspType type);
        void SpawnDroppedCrystal(CrystalColor color);
    }
}