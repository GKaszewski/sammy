using KBCore.Refs;
using Sammy;
using UnityEngine;
using VContainer;

public class PlayerAnimator : MonoBehaviour {
    [SerializeField, Self] private Animator anim;
    [SerializeField, Self] private PlayerMover playerMover;
    [SerializeField] private float dancingTime = 2.5f;
    
    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _walkingHash = Animator.StringToHash("Walk");
    private readonly int _runningHash = Animator.StringToHash("Run");
    private readonly int _jumpingHash = Animator.StringToHash("Jump");
    private readonly int _dancingHash = Animator.StringToHash("Dance");

    private float _lockedTill;
    private int _currentState;
    private bool _dance = false;
    private EventManager _eventManager;

    [Inject]
    public void Construct(EventManager eventManager)
    {
        _eventManager = eventManager;
        _eventManager.OnWin += OnWin;
    }

    private void OnDisable() {
        _eventManager.OnWin -= OnWin;
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
        if (playerMover.IsJumping) return _jumpingHash;
        if (playerMover.IsGrounded && playerMover.IsRunning) return _runningHash;
        if (playerMover.IsGrounded) return playerMover.Intent.magnitude == 0f ? _idleHash : _walkingHash;
        
        return _idleHash;
        
        int LockState(int s, float t) {
            _lockedTill = Time.time + t;
            return s;
        }
    }
}