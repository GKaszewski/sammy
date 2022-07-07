using System;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private bool haveTheSameRotationSpeedAsPlayer = true;
    private RaycastHit camHit;

    public PlayerCharacterController playerCharacterController;
    public Transform parent;
    public Transform target;
    public Transform noRotationCamera;
    public Vector3 offset;

    public float rotationSpeed = 2f;
    public float collisionSensitivity = 4.5f;

    private void Start() {
        if (haveTheSameRotationSpeedAsPlayer)
            rotationSpeed = playerCharacterController.rotationSpeed;
    }

    private void Update() {
        noRotationCamera.localEulerAngles = -transform.localEulerAngles;
    }

    private void LateUpdate() {
        parent.transform.position = target.position;
        if (!playerCharacterController.isJumping) parent.transform.Rotate(Vector3.up * playerCharacterController.input.x * rotationSpeed * Time.deltaTime);
        transform.localPosition = offset;
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