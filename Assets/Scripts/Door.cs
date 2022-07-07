using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum DoorType {
    OPEN_AND_CLOSE,
    OPEN_AND_HOLD
}

public class Door : MonoBehaviour {
    private bool areOpen = false;
    private Vector3 openDestination;
    private Vector3 closeDestination;
    private Inventory playerInventory;

    public CrystalColor doorColor;
    public DoorType doorType;
    public MeshRenderer doorRenderer;

    public Vector3 openOffset;
    public Transform crystalPosition;

    public float movementTime;

    public GameObject crystalPrefab;

    [Tooltip("RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE")]
    public List<Material> doorMaterials = new List<Material>();

    public List<Material> darkerDoorMaterials = new List<Material>();

    private void Start() {
        HandleMaterial();
        CalculateOpenAndClosePosition();
    }

    private void CalculateOpenAndClosePosition() {
        openDestination = transform.position + openOffset;
        closeDestination = transform.position;
    }

    private void HandleMaterial() {
        var materials = doorRenderer.materials;
        switch (doorColor) {
            case CrystalColor.RED:
                materials[0] = doorMaterials[0];
                materials[1] = darkerDoorMaterials[0];
                materials[2] = doorMaterials[0];
                break;
            case CrystalColor.BLUE:
                materials[0] = doorMaterials[1];
                materials[1] = darkerDoorMaterials[1];
                materials[2] = doorMaterials[1];
                break;
            case CrystalColor.GREEN:
                materials[0] = doorMaterials[2];
                materials[1] = darkerDoorMaterials[2];
                materials[2] = doorMaterials[2];
                break;
            case CrystalColor.YELLOW:
                materials[0] = doorMaterials[3];
                materials[1] = darkerDoorMaterials[3];
                materials[2] = doorMaterials[3];
                break;
            case CrystalColor.ORANGE:
                materials[0] = doorMaterials[4];
                materials[1] = darkerDoorMaterials[4];
                materials[2] = doorMaterials[4];
                break;
            case CrystalColor.PURPLE:
                materials[0] = doorMaterials[5];
                materials[1] = darkerDoorMaterials[5];
                materials[2] = doorMaterials[5];
                break;
        }

        doorRenderer.materials = materials;
    }

    private void DestroyCrystal() {
        if (playerInventory) {
            if (playerInventory.reactiveCrystalInfo.Value == CrystalColor.MULTI) return;
            AudioManager.instance.Play("crystal break");
            playerInventory.reactiveCrystalInfo.Value = CrystalColor.NONE;
        }

        SpawnCrystalParticles();
    }

    private void SpawnCrystalParticles() {
    }

    private void ReturnCrystal() {
        if (playerInventory && playerInventory.reactiveCrystalInfo.Value != CrystalColor.MULTI)
            playerInventory.reactiveCrystalInfo.Value = doorColor;
    }

    private void MoveCrystalTowardsDoors() {
        if (!playerInventory) return;

        MoveCrystal(playerInventory.transform.position, crystalPosition.position);
    }

    private void MoveCrystal(Vector3 from, Vector3 to) {
        var crystal = Instantiate(crystalPrefab, from, Quaternion.identity).GetComponent<Crystal>();
        var rb = crystal.GetComponent<Rigidbody>();
        crystal.GetComponent<Collider>().enabled = false;
        crystal.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        crystal.color = doorColor;
        crystal.color = doorColor;
        rb.detectCollisions = false;
        rb.useGravity = false;
        LeanTween.moveLocal(crystal.gameObject, to, movementTime)
            .setOnComplete(() => Destroy(crystal));
    }

    private void MoveCrystalTowardsPlayer() {
        if (!playerInventory) return;

        MoveCrystal(crystalPosition.position, playerInventory.transform.position);
    }

    public void Open() {
        AudioManager.instance.Play("door open");
        if (doorType == DoorType.OPEN_AND_HOLD)
            LeanTween.move(gameObject, openDestination, movementTime).setEaseLinear()
                .setOnComplete(DestroyCrystal);
        else
            LeanTween.move(gameObject, openDestination, movementTime).setEaseLinear()
                .setOnComplete(SpawnCrystalParticles);

        areOpen = true;
    }

    public void Close() {
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, closeDestination, movementTime).setEaseInOutSine().setOnComplete(ReturnCrystal);
        areOpen = false;
    }

    private void HandleDoor(Collider other) {
        if (!other.CompareTag("Player")) return;
        var playerInventory = other.GetComponent<Inventory>();
        if (!playerInventory) return;
        this.playerInventory = playerInventory;
        if (!areOpen && (playerInventory.reactiveCrystalInfo.Value == doorColor ||
                         playerInventory.reactiveCrystalInfo.Value == CrystalColor.MULTI)) Open();
    }

    private void OnTriggerEnter(Collider other) {
        HandleDoor(other);
    }

    private void OnTriggerStay(Collider other) {
        HandleDoor(other);
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player")) return;
        var playerInventory = other.GetComponent<Inventory>();
        if (!playerInventory) return;
        if (doorType == DoorType.OPEN_AND_HOLD) return;
        if (areOpen) Close();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(openDestination, Vector3.one);
        Gizmos.DrawWireCube(closeDestination, Vector3.one);
    }
}