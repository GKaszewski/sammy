using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrystalColor {
    RED,
    BLUE,
    GREEN,
    YELLOW,
    ORANGE,
    PURPLE,
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
}