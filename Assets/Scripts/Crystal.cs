using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CrystalColor {
    RED = 0,
    BLUE = 1,
    GREEN = 2,
    YELLOW = 3,
    ORANGE = 4,
    PURPLE = 5,
    MULTI,
    NONE
}

public class Crystal : BaseCrystal {
    [SerializeField]
    [Range(0f, 1f)]
    private float rotationTime = 0.2f;
    [HideInInspector]
    public Rigidbody rb;
    
    public float force = 2f;
    public float resetRotationTimer = 10f;
    
    public Transform player;
    
    protected void Start() {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    public void Die() {
        AudioManager.instance.Play("crystal break");
        Destroy(gameObject);
    }

    public void ApplyForce() {
        if (!rb) rb = GetComponent<Rigidbody>();
        rb.AddForce((player.forward - transform.up).normalized * force, ForceMode.Impulse);
        StartCoroutine(FixRotation());
    }

    private void ResetRotation() {
        LeanTween.cancel(gameObject);
        LeanTween.rotateLocal(gameObject, Vector3.zero, rotationTime).setOnComplete(() => {
            rb.freezeRotation = true;
        });
    }

    private IEnumerator FixRotation() {
        yield return new WaitForSeconds(resetRotationTimer);
        ResetRotation();
    }
    
    public static CrystalColor Mix(CrystalColor color, CrystalColor otherCrystal) {
        switch (color) {
            case CrystalColor.RED:
                switch (otherCrystal) {
                    case CrystalColor.BLUE:
                        return CrystalColor.PURPLE;
                    case CrystalColor.YELLOW:
                        return CrystalColor.ORANGE;
                }
                break;
            case CrystalColor.BLUE:
                switch (otherCrystal) {
                    case CrystalColor.RED:
                        return CrystalColor.PURPLE;
                    case CrystalColor.YELLOW:
                        return CrystalColor.GREEN;
                }
                break;
            case CrystalColor.YELLOW:
                switch (otherCrystal) {
                    case CrystalColor.BLUE:
                        return CrystalColor.GREEN;
                    case CrystalColor.RED:
                        return CrystalColor.ORANGE;
                }
                break;
            case CrystalColor.MULTI:
                return CrystalColor.MULTI;
        }

        return CrystalColor.NONE;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Ground")) {
            AudioManager.instance.Play("crystal bounce");
        }
    }
}