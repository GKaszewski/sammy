using KBCore.Refs;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private bool haveTheSameRotationSpeedAsPlayer = true;
    private RaycastHit _camHit;
    private bool _won = false;

    [SerializeField, Scene] private PlayerCharacterController playerCharacterController;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform target;
    [SerializeField] private Transform noRotationCamera;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotationSpeed = 2f;
    
    private void Start() {
        GameManager.Instance.eventManager.OnWin += OnWin;
        if (haveTheSameRotationSpeedAsPlayer)
            rotationSpeed = playerCharacterController.rotationSpeed;
    }

    private void OnDisable() {
        GameManager.Instance.eventManager.OnWin -= OnWin;
    }

    private void Update() {
        if (_won) return;
        noRotationCamera.localEulerAngles = -transform.localEulerAngles;
    }

    private void LateUpdate() {
        if (_won) return;
        parent.transform.position = target.position;
        if (!playerCharacterController.isJumping) parent.transform.Rotate(Vector3.up * (playerCharacterController.input.x * rotationSpeed * Time.deltaTime));
        transform.localPosition = offset;
    }

    private void OnWin() {
        _won = true;
    }
    
    private void OnDrawGizmos() {
        Gizmos.DrawCube(_camHit.point, Vector3.one / 3);
    }
}