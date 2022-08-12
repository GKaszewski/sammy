using System;
using Unity.Mathematics;
using UnityEngine;

public class CombatSystem : MonoBehaviour {
    private Inventory inventory;

    public Transform waspSpawner;
    public GameObject waspPrefab;

    private void Start() {
        inventory = GetComponent<Inventory>();
    }

    private void Update() {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.green);
        if (Input.GetButtonDown("Fire1") && inventory.wasps > 0) {
            Shoot();
            inventory.DecreaseWasps();
        }
    }

    private void SpawnNormalWasp() {
        var wasp = Instantiate(waspPrefab, waspSpawner.position,
            quaternion.LookRotation(transform.forward, Vector3.up));
        var rb = wasp.GetComponent<Rigidbody>();
        if (rb) rb.velocity = transform.forward * wasp.GetComponent<Wasp>().speed;
    }

    private void Shoot() {
        switch (inventory.currentWaspType) {
            case WaspType.BASIC:
            case WaspType.NAVIGATING:
                SpawnNormalWasp();
                break;
            case WaspType.THREE:
                break;
        }
    }
}