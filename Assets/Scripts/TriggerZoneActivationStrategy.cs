using Sammy.Interfaces;
using UnityEngine;

namespace Sammy
{
    public class TriggerZoneActivationStrategy : MonoBehaviour, IActivationStrategy
    {
        public bool ShouldActivate(Collider activator) => activator.CompareTag("Player");
    }
}