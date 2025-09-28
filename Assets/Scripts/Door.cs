using System.Collections.Generic;
using KBCore.Refs;
using Sammy;
using UnityEngine;

public enum DoorType {
    OPEN_AND_CLOSE,
    OPEN_AND_HOLD
}

public class Door : MonoBehaviour {
    private Inventory _playerInventory;
    private GameObject _spawnedMovingCrystal;
    
    [SerializeField, Self] private CrystalColorApplier crystalColorApplier;

    protected bool areOpen = false;
    protected Vector3 openDestination;
    protected Vector3 closeDestination;
    protected float openDistance;
    protected float closeDistance;

    public CrystalColor doorColor;
    public DoorType doorType;

    public Vector3 openOffset;
    public Transform crystalPosition;

    public float movementTime;

    public GameObject crystalPrefab;

    private void Start() {
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

    private void DestroyCrystal() {
        if (_playerInventory) {
            if (_playerInventory.ReactiveCrystalInfo.Value == CrystalColor.MULTI) return;
            AudioManager.Instance.Play("crystal break");
            _playerInventory.ReactiveCrystalInfo.Value = CrystalColor.NONE;
            doorColor = CrystalColor.NONE;
            crystalColorApplier.ApplyMaterial(doorColor);
        }

        areOpen = true;
        SpawnCrystalParticles();
    }

    private void SpawnCrystalParticles() {
    }

    private void ReturnCrystal() {
        if (_playerInventory && _playerInventory.ReactiveCrystalInfo.Value != CrystalColor.MULTI && doorType == DoorType.OPEN_AND_HOLD)
            _playerInventory.ReactiveCrystalInfo.Value = doorColor;
    }

    private void MoveCrystalTowardsDoors() {
        if (!_playerInventory) return;
        var crystal = Instantiate(crystalPrefab, _playerInventory.crystalSpawnPosition.position, Quaternion.identity).GetComponent<MovingCrystal>();
        crystal.color = doorColor;
        LeanTween.move(crystal.gameObject, crystalPosition, openDistance / movementTime)
            .setOnComplete(() => Destroy(crystal.gameObject));

        _spawnedMovingCrystal = crystal.gameObject;
    }

    private void MoveCrystalTowardsPlayer() {
        if (!_playerInventory || !_spawnedMovingCrystal) return;
        LeanTween.cancel(_spawnedMovingCrystal);
        LeanTween.move(_spawnedMovingCrystal, _playerInventory.crystalSpawnPosition,  closeDistance / movementTime)
            .setOnComplete(() => Destroy(_spawnedMovingCrystal));
    }

    public virtual void Open() {
        AudioManager.Instance.Play("door open");
        LeanTween.cancel(gameObject);
        if (doorType == DoorType.OPEN_AND_HOLD) {
            _playerInventory.previousColor = _playerInventory.ReactiveCrystalInfo.Value;
            _playerInventory.ReactiveCrystalInfo.Value = CrystalColor.NONE;
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
        this._playerInventory = playerInventory;
        if (!areOpen && (playerInventory.ReactiveCrystalInfo.Value == doorColor ||
                         playerInventory.ReactiveCrystalInfo.Value == CrystalColor.MULTI)) Open();
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