using System;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private bool haveTheSameRotationSpeedAsPlayer = true;
    private RaycastHit camHit;
    private bool won = false;

    public PlayerCharacterController playerCharacterController;
    public Transform parent;
    public Transform target;
    public Transform noRotationCamera;
    public Vector3 offset;

    public float rotationSpeed = 2f;
    private void Start() {
        GameManager.Instance.eventManager.OnWin += OnWin;
        if (haveTheSameRotationSpeedAsPlayer)
            rotationSpeed = playerCharacterController.rotationSpeed;
    }

    private void OnDisable() {
        GameManager.Instance.eventManager.OnWin -= OnWin;
    }

    private void Update() {
        if (won) return;
        noRotationCamera.localEulerAngles = -transform.localEulerAngles;
    }

    private void LateUpdate() {
        if (won) return;
        parent.transform.position = target.position;
        if (!playerCharacterController.isJumping) parent.transform.Rotate(Vector3.up * playerCharacterController.input.x * rotationSpeed * Time.deltaTime);
        transform.localPosition = offset;
    }

    private void OnWin() {
        won = true;
    }
    
    private void HandleCameraCollision() {
        Debug.DrawRay(transform.position, -noRotationCamera.forward * 1f, Color.red);
        if (Physics.Raycast(transform.position, -noRotationCamera.forward, out camHit, 1f)) {
            transform.position = camHit.point;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
                transform.localPosition.z + Vector3.Distance(transform.position, camHit.point));
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawCube(camHit.point, Vector3.one / 3);
    }
}