using KBCore.Refs;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    [SerializeField, Self] private Animator anim;
    [SerializeField, Self] private PlayerCharacterController playerCharacterController;
    [SerializeField] private float dancingTime = 2.5f;
    
    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _walkingHash = Animator.StringToHash("Walk");
    private readonly int _runningHash = Animator.StringToHash("Run");
    private readonly int _jumpingHash = Animator.StringToHash("Jump");
    private readonly int _dancingHash = Animator.StringToHash("Dance");

    private float _lockedTill;
    private int _currentState;
    private bool _dance = false;
    
    private void Start() {
        GameManager.Instance.eventManager.OnWin += OnWin;
    }

    private void OnDisable() {
        GameManager.Instance.eventManager.OnWin -= OnWin;
    }

    private void OnWin() {
        _dance = true;
    }

    private void Update() {
        var state = GetState();
        _dance = false;

        if (state == _currentState) return;
        anim.CrossFade(state, 0.1f, 0);
        _currentState = state;
    }

    private int GetState() {
        if (Time.time < _lockedTill) return _currentState;
        if (_dance) return LockState(_dancingHash, dancingTime);
        if (playerCharacterController.isJumping) return _jumpingHash;
        if (playerCharacterController.isGrounded && playerCharacterController.isRunning) return _runningHash;
        if (playerCharacterController.isGrounded) return playerCharacterController.intent.magnitude == 0f ? _idleHash : _walkingHash;
        
        return _idleHash;
        
        int LockState(int s, float t) {
            _lockedTill = Time.time + t;
            return s;
        }
    }
}