using System;
using UniRx;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public int points;
    public int wasps = 10;
    public ReactiveProperty<CrystalColor> reactiveCrystalInfo;
    public WaspType currentWaspType;

    public GameObject crystalPrefab;
    public Transform crystalSpawnPosition;
    [HideInInspector]
    public CrystalColor previousColor;

    private void Start() {
        reactiveCrystalInfo = new ReactiveProperty<CrystalColor>(CrystalColor.NONE);
        reactiveCrystalInfo.Subscribe(color => GameManager.Instance.eventManager.ChangeCrystal(color));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            reactiveCrystalInfo.Value = CrystalColor.NONE;
        }
    }

    public void CollectPoints(int _points) {
        points += _points;
        if (points >= GameManager.Instance.maxPoints) {
            reactiveCrystalInfo.Value = CrystalColor.MULTI;
            AudioManager.instance.Play("Won");
        }
        GameManager.Instance.eventManager.PickPointUp(points);
    }
    
    private Crystal SpawnCrystal(CrystalColor color, Vector3 position) {
        AudioManager.instance.Play("crystal spawn");
        var newCrystal = Instantiate(crystalPrefab, position, Quaternion.identity).GetComponent<Crystal>();
        newCrystal.color = color;
        newCrystal.HandleMaterial();

        return newCrystal;
    }

    public void DecreaseWasps() {
        wasps--;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (!hit.collider.CompareTag("Crystal")) return;
        var newCrystal = hit.gameObject.GetComponent<Crystal>();
        if (!newCrystal) return;
        AudioManager.instance.Play("crystal pickup");
        if (reactiveCrystalInfo.Value != CrystalColor.NONE && reactiveCrystalInfo.Value != CrystalColor.MULTI) {
            var newColor = Crystal.Mix(reactiveCrystalInfo.Value, newCrystal.color);
            if (newColor == CrystalColor.NONE) {
                var spawnedCrystal = SpawnCrystal(reactiveCrystalInfo.Value, crystalSpawnPosition.position + (transform.forward * 2f));
                spawnedCrystal.player = transform;
                spawnedCrystal.ApplyForce();
            }
            else
                AudioManager.instance.Play("crystal mix");

            reactiveCrystalInfo.Value = newColor != CrystalColor.NONE ? newColor : newCrystal.color;
        }
        else if (reactiveCrystalInfo.Value == CrystalColor.NONE) {
            reactiveCrystalInfo.Value = newCrystal.color;
        }
        GameManager.Instance.eventManager.PickCrystalUp(reactiveCrystalInfo.Value);
        newCrystal.Die();
    }

    public void DropCrystal() {
        if (reactiveCrystalInfo.Value == CrystalColor.NONE) return;
        var spawnedCrystal = SpawnCrystal(reactiveCrystalInfo.Value, crystalSpawnPosition.position + (transform.forward * 2f));
        spawnedCrystal.player = transform;
        spawnedCrystal.ApplyForceUp();
        AudioManager.instance.Play("crystal spawn");
        reactiveCrystalInfo.Value = CrystalColor.NONE;
    }
}