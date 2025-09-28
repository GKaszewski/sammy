using UnityEngine;
using VContainer;

namespace Sammy
{
    public class PlayerCrystalHandler : MonoBehaviour
    {
        private Inventory _inventory;
        private CrystalManager _crystalManager;
        private EventManager _eventManager;
        private EffectsManager _effectsManager;
        
        [Tooltip("The crystal prefab to spawn when dropping.")]
        [SerializeField] private GameObject crystalPrefab;
        
        [Inject]
        public void Construct(Inventory inventory, CrystalManager crystalManager, EventManager eventManager, EffectsManager effectsManager)
        {
            _inventory = inventory;
            _crystalManager = crystalManager;
            _eventManager = eventManager;
            _effectsManager = effectsManager;
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!hit.collider.CompareTag("Crystal")) return;
            if (!hit.gameObject.TryGetComponent(out Crystal newCrystal)) return;

            AudioManager.Instance.Play("crystal pickup");
            _effectsManager.SpawnEffect(EffectType.POOF, hit.point);

            var currentColor = _inventory.ReactiveCrystalInfo.Value;

            if (currentColor != CrystalColor.NONE && currentColor != CrystalColor.MULTI)
            {
                var mixedColor = _crystalManager.Mix(currentColor, newCrystal.color);

                if (mixedColor == CrystalColor.NONE)
                {
                    SpawnCrystal(currentColor);
                    _inventory.ReactiveCrystalInfo.Value = newCrystal.color;
                }
                else
                {
                    AudioManager.Instance.Play("crystal mix");
                    _inventory.ReactiveCrystalInfo.Value = mixedColor;
                }
            }
            else if (currentColor == CrystalColor.NONE)
            {
                _inventory.ReactiveCrystalInfo.Value = newCrystal.color;
            }

            _eventManager.PickCrystalUp(_inventory.ReactiveCrystalInfo.Value);
            newCrystal.Die();
        }
        
        public void DropCrystal()
        {
            if (_inventory.ReactiveCrystalInfo.Value == CrystalColor.NONE) return;

            var spawnedCrystal = SpawnCrystal(_inventory.ReactiveCrystalInfo.Value);
            spawnedCrystal.ApplyForceUp();
        
            AudioManager.Instance.Play("crystal spawn");
            _inventory.ReactiveCrystalInfo.Value = CrystalColor.NONE;
        }
        
        private Crystal SpawnCrystal(CrystalColor color)
        {
            Vector3 spawnPos = _inventory.crystalSpawnPosition.position + (transform.forward * 2f);
            var newCrystalGo = Instantiate(crystalPrefab, spawnPos, Quaternion.identity);
            var newCrystal = newCrystalGo.GetComponent<Crystal>();
        
            newCrystal.color = color;
            newCrystal.HandleMaterial();
            newCrystal.player = transform;
        
            AudioManager.Instance.Play("crystal spawn");
            return newCrystal;
        }
    }
}