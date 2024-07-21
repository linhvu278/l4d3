using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    
    private Vector2 movementInput;
    PlayerManager playerManager;

    [SerializeField] CharacterController controller;

    private float currentSpeed;
    private const float normalSpeed = 5f;
    private const float sprintSpeedMultiplier = 1.55f;
    private const float crouchSpeedMultiplier = 0.45f;

    // [SerializeField] Text staminaText;
    private float stamina;
    private const float JUMP_STAMINA = 18f;
    private const float SPRINT_STAMINA = 15f;
                
    private const float gravity = -9.81f;
    private Vector3 velocity;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded;
    private const float GROUND_HEIGHT = 0.1f;

    [SerializeField] float jumpHeight;
    private bool isJumping, isSprinting, isCrouching, isAiming;
    private bool canMove, canJump, canSprint;

    // [SerializeField] Transform cam;

    public void Jump() => isJumping = true;
    public void StartSprint() => isSprinting = true;
    public void StopSprint() => isSprinting = false;
    public void Crouch() => isCrouching = !isCrouching;

    public bool IsGrounded => isGrounded;
    // public bool IsSprinting => isSprinting;
    // public bool IsJumping => isJumping;
    public bool IsMoving => movementInput != Vector2.zero;
    public bool IsAiming { get => isAiming; set => isAiming = value; }
    public bool CanMove { get; set; }
    public bool CanSprint { get; set; }
    public bool CanJump { get; set; }

    public float SprintSpeedMultiplier(bool value) => value ? sprintSpeedMultiplier : 1f;
    public float CrouchSpeedMultiplier(bool value) => value ? crouchSpeedMultiplier : 1f;

    void Update(){
        stamina = playerManager.Stamina;
        canMove = CanMove;
        canJump = CanJump && isGrounded && stamina >= JUMP_STAMINA;
        canSprint = CanSprint && canMove && isGrounded && movementInput.y > 0 && stamina > 0;

        // player movement
        currentSpeed = normalSpeed * SprintSpeedMultiplier(isSprinting) * CrouchSpeedMultiplier(isCrouching || isAiming);
        if (canMove){
            Vector3 movement = transform.right * movementInput.x + transform.forward * movementInput.y;
            controller.Move(movement * currentSpeed * Time.deltaTime);
        }

        // player gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, GROUND_HEIGHT, groundMask);
        if (isGrounded && velocity.y < 0f){
            velocity.y = 0;
        }

        // player jumping
        if (isJumping){
            if (canJump){
                velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
                playerManager.StaminaDrain(JUMP_STAMINA);
            }
            isJumping = false;
        }

        // player sprinting
        if (isSprinting && canSprint){
            // currentSpeed = sprintSpeedMultiplier;
            playerManager.StaminaDrain(SPRINT_STAMINA * Time.deltaTime);
        } else {
            StopSprint();
            // currentSpeed = normalSpeed;
            playerManager.StaminaRegen();
        }

        // crouching
        // if (isCrouching)
        // {
        //     Debug.Log("crouching");
        //     controller.height = 0.4f;
        // }
        // else controller.height = 2f;
    }

    void Start(){
        playerManager = GetComponent<PlayerManager>();

        CanMove = true;
        CanJump = true;
        CanSprint = true;
    }
    void Awake(){
        if (instance != null){
            Debug.Log("More than one instances of PlayerMovement found");
            return;
        }
        instance = this;
    }
    
    public void ReceiveInput(Vector2 value){
        movementInput = value;
    }
}