using System;
using UnityEngine;

public class WaspPickup : MonoBehaviour {
    private Inventory inventory;
    private CombatSystem combatSystem;
    private PlayerUIManager playerUIManager;

    private void Start() {
        inventory = GetComponent<Inventory>();
        combatSystem = GetComponent<CombatSystem>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();
    }

    private void ResetWaspsInInventory() {
        inventory.wasps = 0;
    }

    private void SetNewWasps(WaspData data) {
        inventory.currentWaspType = data.type;
        inventory.wasps += data.quantity;
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
            playerUIManager.SetWaspAvatar();
            Destroy(hit.gameObject);
            return;
        }

        inventory.wasps += waspData.quantity;
        playerUIManager.SetWaspAvatar();
        Destroy(hit.gameObject);
    }
}
