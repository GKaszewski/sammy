using UnityEngine;

public class PlayerCharacterController : MonoBehaviour {
    private Animator anim;
    private CharacterController cc;
    private RaycastHit slopeHit;
    private Vector3 slopeDirection;
    private Vector3 camF;
    private Vector3 camR;
    [HideInInspector]
    public Vector3 intent;
    private Vector3 velocityXZ;
    private Vector3 velocity;
    private Vector3 hitNormal;
    private float currentSpeed = 0f;
    private float currentStamina = 0f;
    private bool canRun = false;
    private bool isSliding = false;
    private bool intentToJump = false;

    private const float GRAVITY = 10;

    [Range(0, 1f)] public float animSpeed = 0.0f;
    public float walkSpeed = 5f;
    public float runSpeed = 7f;
    public float jumpForce = 5f;
    public float slideSpeed = 10f;
    public float rotationSpeed = 5f;
    public float quickRotationTime = 0.2f;
    public float maxStamina = 100f;
    public float playerHeight = 2f;
    public Transform feet;
    public float groundDetectionRadius = 0.1f;
    [HideInInspector] public Vector2 input;
    public bool isJumping = false;
    [HideInInspector]
    public bool ccIsGrounded = false;
    public bool isGrounded = false;
    [HideInInspector]
    public bool isRunning = false;
    public Transform cameraParent;
    public LayerMask groundDetectionLayerMask;

    private void Start() {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
        currentStamina = maxStamina;
    }

    private void Update() {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        anim.SetFloat("Speed", animSpeed);
        ccIsGrounded = cc.isGrounded;
        HandleCamera();
        CheckRun();
        HandleRunning();
        HandleQuickTurn();

        RestartStaminaWithKey();
        HandleAnimatorSpeed();

        CheckGround();
        CalculateMovement();
        ApplyGravity();
        HandleJumping();
        if (isSliding) {
            velocity.x += ((1f - hitNormal.y) * hitNormal.x) * slideSpeed;
            velocity.z += ((1f - hitNormal.y) * hitNormal.z) * slideSpeed;
            slopeDirection = Vector3.ProjectOnPlane(velocity, hitNormal);
            cc.Move(slopeDirection * Time.deltaTime);
        }
        else {
            cc.Move(velocity * Time.deltaTime);
        }
        
        CheckSliding();

        if (input.magnitude > 0 && !isJumping) transform.Rotate(Vector3.up * input.x * rotationSpeed * Time.deltaTime);

        Debug.DrawRay(transform.position, intent, Color.magenta);
    }

    private void CheckGround() {
        var colliders = Physics.OverlapSphere(feet.position, groundDetectionRadius, groundDetectionLayerMask);
        isGrounded = colliders.Length > 0;
    }

    private void ApplyGravity() {
        if (isGrounded && !intentToJump) velocity.y = -0.5f;
        else velocity.y -= GRAVITY * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -10, jumpForce);
    }

    private void CalculateMovement() {
        intent = camF * input.y + camR * input.x;
        velocityXZ = velocity;
        velocityXZ.y = 0;
        velocityXZ = transform.forward * currentSpeed * input.y;
        velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
    }

    private void CheckSliding() {
        isSliding = Vector3.Angle(Vector3.up, hitNormal) > cc.slopeLimit && isGrounded;
    }

    private void HandleCamera() {
        camF = cameraParent.forward;
        camR = cameraParent.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;
    }

    private void HandleJumping() {
        isJumping = velocity.y > 0 && !isGrounded;
        intentToJump = Input.GetButton("Jump");
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = jumpForce;
        }
    }

    private void HandleQuickTurn() {
        if (Input.GetButtonDown("Quick turn") && Input.GetAxisRaw("Horizontal") > 0) {
            AudioManager.instance.Play("quickturn");
            QuickTurn(90f);
            return;
        }
        else if (Input.GetButtonDown("Quick turn") && Input.GetAxisRaw("Horizontal") < 0) {
            AudioManager.instance.Play("quickturn");
            QuickTurn(-90f);
            return;
        }

        if (Input.GetButtonDown("Quick turn")) {
            AudioManager.instance.Play("quickturn");
            QuickTurn(90f);
        }
    }

    private void HandleAnimatorSpeed() {
        if (input.y != 0 && !canRun)
            animSpeed = 0.6f;
        else if (input.y == 0 && !canRun)
            animSpeed = 0f;
    }

    private void RestartStaminaWithKey() {
        if (Input.GetKey(KeyCode.R)) RestartStamina();
    }

    private void RestartStamina() => currentStamina = maxStamina;

    private void HandleRunning() {
        if (canRun) {
            isRunning = true;
            currentSpeed = runSpeed;
            animSpeed = 1f;
            currentStamina -= Time.time * Time.deltaTime;
        }
        else {
            isRunning = false;
            currentSpeed = walkSpeed;
            animSpeed = 0.6f;
        }
    }

    private void CheckRun() {
        canRun = Input.GetButton("Run") && currentStamina > 0 && Mathf.Abs(input.y) > 0.0f;
    }


    private void QuickTurn(float angle) {
        LeanTween.cancel(gameObject);
        LeanTween.rotateAroundLocal(cameraParent.gameObject, Vector3.up, angle, quickRotationTime);
        LeanTween.rotateAroundLocal(gameObject, Vector3.up, angle, quickRotationTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        hitNormal = hit.normal;
    }
}