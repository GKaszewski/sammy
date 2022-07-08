using UnityEngine;


public class ExitDoor : Door {
    public int pointsToUnlock;


    public override void Open() {
        AudioManager.instance.Play("door open");
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, openDestination, openDistance / movementTime).setEaseLinear();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        var playerInventory = other.GetComponent<Inventory>();
        if (!playerInventory) return;

        if (playerInventory.points >= pointsToUnlock || playerInventory.reactiveCrystalInfo.Value == CrystalColor.MULTI) {
            Open();
        }
    }
}