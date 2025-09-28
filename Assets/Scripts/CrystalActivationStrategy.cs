using Sammy.Interfaces;
using UnityEngine;

namespace Sammy
{
    public class CrystalActivationStrategy : MonoBehaviour, IActivationStrategy
    {
        [SerializeField] private CrystalColor requiredColor;
        
        public bool ShouldActivate(Collider activator)
        {
            var inventory = activator.GetComponent<Inventory>();
            return inventory != null && (inventory.ReactiveCrystalInfo.Value == requiredColor || inventory.ReactiveCrystalInfo.Value == CrystalColor.MULTI);
        }
    }
}