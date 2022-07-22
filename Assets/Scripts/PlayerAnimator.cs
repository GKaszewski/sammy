using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    private Animator anim;
    
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int WalkingHash = Animator.StringToHash("Walk");
    private readonly int RunningHash = Animator.StringToHash("Run");
    private readonly int JumpingHash = Animator.StringToHash("Jump");
    private readonly int DancingHash = Animator.StringToHash("Dance");

    private float lockedTill;
    private int currentState;

    private bool dance = false;

    private PlayerCharacterController playerCharacterController;

    public float dancingTime = 2.5f;
    
    private void Start() {
        anim = GetComponent<Animator>();
        GameManager.Instance.eventManager.OnWin += OnWin;
        playerCharacterController = GetComponent<PlayerCharacterController>();
    }

    private void OnDisable() {
        GameManager.Instance.eventManager.OnWin -= OnWin;
    }

    private void OnWin() {
        dance = true;
    }

    private void Update() {
        var state = GetState();
        dance = false;

        if (state == currentState) return;
        anim.CrossFade(state, 0.1f, 0);
        currentState = state;
    }

    private int GetState() {
        if (Time.time < lockedTill) return currentState;
        if (dance) return LockState(DancingHash, dancingTime);
        if (playerCharacterController.isJumping) return JumpingHash;
        if (playerCharacterController.isGrounded && playerCharacterController.isRunning) return RunningHash;
        if (playerCharacterController.isGrounded) return playerCharacterController.intent.magnitude == 0f ? IdleHash : WalkingHash;
        
        return IdleHash;
        
        int LockState(int s, float t) {
            lockedTill = Time.time + t;
            return s;
        }
    }
}