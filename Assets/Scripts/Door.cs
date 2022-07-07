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
    private float openDistance;
    private float closeDistance;
    private Inventory playerInventory;

    private GameObject spawnedMovingCrystal;

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

    private void Update() {
        openDistance = Vector3.Distance(transform.position, openDestination);
        closeDistance = Vector3.Distance(transform.position, closeDestination);
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

        areOpen = true;
        SpawnCrystalParticles();
    }

    private void SpawnCrystalParticles() {
    }

    private void ReturnCrystal() {
        if (playerInventory && playerInventory.reactiveCrystalInfo.Value != CrystalColor.MULTI && doorType == DoorType.OPEN_AND_HOLD)
            playerInventory.reactiveCrystalInfo.Value = doorColor;
    }

    private void MoveCrystalTowardsDoors() {
        if (!playerInventory) return;
        var crystal = Instantiate(crystalPrefab, playerInventory.crystalSpawnPosition.position, Quaternion.identity).GetComponent<MovingCrystal>();
        crystal.color = doorColor;
        LeanTween.move(crystal.gameObject, crystalPosition, openDistance / movementTime)
            .setOnComplete(() => Destroy(crystal.gameObject));

        spawnedMovingCrystal = crystal.gameObject;
    }

    private void MoveCrystalTowardsPlayer() {
        if (!playerInventory || !spawnedMovingCrystal) return;
        LeanTween.cancel(spawnedMovingCrystal);
        LeanTween.move(spawnedMovingCrystal, playerInventory.crystalSpawnPosition,  closeDistance / movementTime)
            .setOnComplete(() => Destroy(spawnedMovingCrystal));
    }

    public void Open() {
        AudioManager.instance.Play("door open");
        LeanTween.cancel(gameObject);
        if (doorType == DoorType.OPEN_AND_HOLD) {
            playerInventory.previousColor = playerInventory.reactiveCrystalInfo.Value;
            playerInventory.reactiveCrystalInfo.Value = CrystalColor.NONE;
            MoveCrystalTowardsDoors();
            LeanTween.move(gameObject, openDestination, openDistance / movementTime).setEaseLinear()
                .setOnComplete(DestroyCrystal);
        } else {
            LeanTween.move(gameObject, openDestination, openDistance / movementTime).setEaseLinear()
                .setOnComplete(SpawnCrystalParticles);
            areOpen = true;
        }

       
    }

    public void Close() {
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, closeDestination, closeDistance / movementTime).setEaseInOutSine().setOnComplete(ReturnCrystal);
        if (doorType == DoorType.OPEN_AND_HOLD) MoveCrystalTowardsPlayer();
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
        if (doorType == DoorType.OPEN_AND_HOLD && !areOpen && doorColor == playerInventory.previousColor) {
            Close();
            return;
        }
        if (areOpen && doorType != DoorType.OPEN_AND_HOLD) Close();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(openDestination, Vector3.one);
        Gizmos.DrawWireCube(closeDestination, Vector3.one);
    }
}