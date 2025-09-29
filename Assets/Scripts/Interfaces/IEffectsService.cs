using UnityEngine;

namespace Sammy.Interfaces
{
    public interface IEffectsService
    {
        void SpawnEffect(EffectType type, Vector3 position);
    }
}