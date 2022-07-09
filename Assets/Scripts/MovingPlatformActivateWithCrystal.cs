using System;
using UnityEngine;

public class MovingPlatformActivateWithCrystal : MovingPlatform {
    private Inventory inventory;
    private MeshRenderer renderer;

    public CrystalColor activationCrystal;
    public Material[] platformMaterials;
    protected void Start() {
        base.Start();
        renderer = GetComponent<MeshRenderer>();
        HandleMaterial();
    }
    protected void Update() {
        base.Update();
        if (inventory) {
            var hasCorrectCrystal = inventory.reactiveCrystalInfo.Value == activationCrystal;
            if (isOn && isMoving && !hasCorrectCrystal) isOn = false;
        }
    }
    
    public void HandleMaterial() {
        switch (activationCrystal) {
            case CrystalColor.RED:
                renderer.material = platformMaterials[0];
                break;
            case CrystalColor.BLUE:
                renderer.material = platformMaterials[1];
                break;
            case CrystalColor.GREEN:
                renderer.material = platformMaterials[2];
                break;
            case CrystalColor.YELLOW:
                renderer.material = platformMaterials[3];
                break;
            case CrystalColor.ORANGE:
                renderer.material = platformMaterials[4];
                break;
            case CrystalColor.PURPLE:
                renderer.material = platformMaterials[5];
                break;
        }
    }

    private void ActivatePlatform(Collider other) {
        if (other.CompareTag("Player")) {
            inventory = other.GetComponent<Inventory>();
            if (inventory.reactiveCrystalInfo.Value == activationCrystal) isOn = true;
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        ActivatePlatform(other);
    }

    protected void OnTriggerStay(Collider other) {
        base.OnTriggerEnter(other);
        ActivatePlatform(other);
    }

    protected override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        if (other.CompareTag("Player")) isOn = false;
    }
}