using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    
    private Vector2 movementInput;
    PlayerManager playerManager;

    [SerializeField] CharacterController controller;

    private float currentSpeed;
    private const float NORMAL_SPEED = 5f;
    private const float SPRINT_SPEED = 10f;
    private const float CROUCH_SPEED = 2f;

    // [SerializeField] Text staminaText;
    private float stamina;
    private const float JUMP_STAMINA = 15f;
    private const float SPRINT_STAMINA = 12f;
                
    private const float gravity = -9.81f;
    private Vector3 velocity;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded;
    private const float GROUND_HEIGHT = 0.1f;

    [SerializeField] float jumpHeight;
    private bool isJumping, isSprinting, isCrouching;
    private bool canMove, canJump, canSprint;

    // [SerializeField] Transform cam;

    void Update(){
        stamina = playerManager.Stamina;
        canJump = isGrounded && stamina >= JUMP_STAMINA;
        canSprint = canMove && isGrounded && stamina > 0;

        // player movement
        if (canMove){
            Vector3 movement = transform.right * movementInput.x + transform.forward * movementInput.y;
            controller.Move(movement * currentSpeed * Time.deltaTime);
        }

        // player gravity
        velocity.y = velocity.y + gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, GROUND_HEIGHT, groundMask);
        if (isGrounded && velocity.y < 0f){
            velocity.y = 0;
        }

        // player jumping
        // canJump = isGrounded && stamina >= JUMP_STAMINA;
        if (isJumping){
            if (canJump){
                velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
                playerManager.StaminaDrain(JUMP_STAMINA);
            }
            isJumping = false;
        }

        // player sprinting
        if (isSprinting && canSprint && movementInput.y > 0){
            currentSpeed = SPRINT_SPEED;
            playerManager.StaminaDrain(SPRINT_STAMINA * Time.deltaTime);
        } else {
            StopSprint();
            currentSpeed = NORMAL_SPEED;
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

    public void Jump() => isJumping = true;
    public void StartSprint() => isSprinting = true;
    public void StopSprint() => isSprinting = false;
    public void Crouch() => isCrouching = !isCrouching;

    public bool IsGrounded => isGrounded;
    public bool IsSprinting => isSprinting;
    // public bool IsJumping => isJumping;
    public bool IsMoving => movementInput != new Vector2(0,0);
    // public bool CanMove { get => canMove; set => canMove = value; }
    // public bool CanSprint => canMove && isGrounded && stamina > 0;
    // public bool CanJump { get => isGrounded && stamina >= JUMP_STAMINA && canJump; set => canJump = value; }

    void Start(){
        playerManager = GetComponent<PlayerManager>();

        // CanMove = true;
        // CanJump = true;
    }
    void Awake(){
        if (instance != null){
            Debug.Log("More than one instances of PlayerMovement found");
            return;
        }
        instance = this;
    }
    
    public void ReceiveInput(Vector2 horizontalValue, bool value){
        movementInput = horizontalValue;
        canMove = value;
    }
}