using UnityEngine;


public class ExitDoor : Door {
    public int pointsToUnlock;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        var playerInventory = other.GetComponent<Inventory>();
        if (!playerInventory) return;

        if (playerInventory.points >= pointsToUnlock || playerInventory.reactiveCrystalInfo.Value == CrystalColor.MULTI) {
            Open();
        }
    }
}