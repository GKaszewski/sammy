using System;
using KBCore.Refs;
using UnityEngine;
using VContainer;

public class WaspPickup : MonoBehaviour {
    private PlayerUIManager _playerUIManager;
    
    [SerializeField, Self] private Inventory inventory;
    [SerializeField, Self] private CombatSystem combatSystem;
    
    [Inject]
    public void Construct(PlayerUIManager playerUIManager) {
        _playerUIManager = playerUIManager;
    }

    private void ResetWaspsInInventory() {
        inventory.SetWasps(0);
    }

    private void SetNewWasps(WaspData data) {
        inventory.currentWaspType = data.type;
        inventory.AddWasps(data.quantity);
        combatSystem.waspPrefab = data.prefab;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (!hit.gameObject.CompareTag("Wasp pickup")) return;
        var waspData = hit.gameObject.GetComponent<WaspData>();
        if (!waspData) return;
        var currentType = inventory.currentWaspType;
        if (currentType != waspData.type) {
            ResetWaspsInInventory();
            SetNewWasps(waspData);
            _playerUIManager.SetWaspAvatar();
            Destroy(hit.gameObject);
            return;
        }

        inventory.AddWasps(waspData.quantity);
        _playerUIManager.SetWaspAvatar();
        Destroy(hit.gameObject);
    }
}
