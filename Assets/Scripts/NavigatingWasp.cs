using System;
using UnityEngine;

public class NavigatingWasp : Wasp {
    private Rigidbody rb;

    public Transform target;

    protected void Start() {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (!target) return;
        var direction = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other) {
        if (!target && other.CompareTag("Lion")) {
            target = other.transform;
        }
    }
}