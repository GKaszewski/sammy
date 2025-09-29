using KBCore.Refs;
using Sammy;
using UnityEngine;
using VContainer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool haveTheSameRotationSpeedAsPlayer = true;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform target;
    [SerializeField] private Transform noRotationCamera;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotationSpeed = 2f;
    
    private RaycastHit _camHit;
    private bool _won = false;
    private EventManager _eventManager;
    private PlayerMover _playerMover;
    private PlayerInput _playerInput;


    [Inject]
    private void Construct(EventManager eventManager, PlayerMover playerMover, PlayerInput playerInput)
    {
        _eventManager = eventManager;
        _playerMover = playerMover;
        _playerInput = playerInput;
    }

    private void Start()
    {
        _eventManager.OnWin += OnWin;
        if (haveTheSameRotationSpeedAsPlayer)
            rotationSpeed = _playerMover.RotationSpeed;
    }

    private void OnDisable()
    {
        _eventManager.OnWin -= OnWin;
    }

    private void Update()
    {
        if (_won) return;
        noRotationCamera.localEulerAngles = -transform.localEulerAngles;
    }

    private void LateUpdate()
    {
        if (_won) return;
        parent.transform.position = target.position;
        if (!_playerMover.IsJumping)
            parent.transform.Rotate(Vector3.up * (_playerInput.MoveInput.x * rotationSpeed * Time.deltaTime));
        transform.localPosition = offset;
    }

    private void OnWin()
    {
        _won = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_camHit.point, Vector3.one / 3);
    }
}