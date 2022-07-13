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
    
    private PlayerCharacterController playerCharacterController;

    private void Start() {
        anim = GetComponent<Animator>();
        playerCharacterController = GetComponent<PlayerCharacterController>();
    }

    private void Update() {
        var state = GetState();

        if (state == currentState) return;
        anim.CrossFade(state, 0.1f, 0);
        currentState = state;
    }

    private int GetState() {
        if (Time.time < lockedTill) return currentState;
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