using UnityEngine;
using Unity.Netcode;

public class FilipMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    
    [SerializeField] private float gravity;
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float jumpForce;
    [SerializeField] private float sprintMultiplier;

    private bool isWalking; 
    private bool isRunning; 
    private bool isJumping; 

    private readonly float turnSmoothTime = 0.1f; 
    private float turnSmoothVelocity;
    
    private Vector3 velocity;

    void Awake() 
    {
        // Hide the cursor.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        //if(!IsOwner) return;
        
        // Call movement functions.
        ApplyGravity();  
        Jump();  
        Move();

        // NOTE: Should always get called as last thing!
        HandleAnimations();
        // Apply velocity.
        controller.Move(velocity * Time.deltaTime); 
    }

    // This function is responsible for applying forces to the player.
    public void ApplyForce(Vector3 force)
    {
        velocity = velocity + force * Time.deltaTime;
    }
    
    // This function is responsible for applying gravity to the player.
    private void ApplyGravity() 
    {   
        // Gravity is applied to the player if it isn't already grounded.
        if (!controller.isGrounded) ApplyForce(new Vector3(0, -gravity));
        // Otherwise the player is still pushed slightly downwards to ensure it stays grounded.
        else velocity.y = -1f;
        // This is to ensure the ground check works when the jump-function is called.
    }

    // This function is responsible for making the character move.
    private void Move() 
    {
        // Get input.
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift)) isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) isRunning = false;

        float movementMultiplier = 1;

        if (isRunning)
        {
        	movementMultiplier *= sprintMultiplier; 
	    }

        // Normalize the direction vector.
        Vector3 direction = new Vector3(inputHorizontal, 0, inputVertical).normalized;

        // Don't move, if the direction vector's magnitude is too close to zero.
        if (direction.magnitude >= 0.1)
        {
            isWalking = true;

            // Calculate the player's look direction.
            float lookDirectionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Smooth the rotation of the player model.
            float playerAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookDirectionAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, playerAngle, 0f);

            // The movement direction is calculated using the look direction.
            Vector3 moveDirection = Quaternion.Euler(0.0f, lookDirectionAngle, 0.0f) * Vector3.forward;

            // Apply movement to the player.
		    controller.Move(moveDirection.normalized * moveSpeed * movementMultiplier * Time.deltaTime);

            return;
        }

        isWalking = false;
    }

    // This function is responsible for making the character jump.
    private void Jump() 
    {   
        // If the "Jump" button is pressed...
        if (Input.GetButtonDown("Jump"))
        {
            // Check if the character is on the ground. If not, do nothing.
            if (!controller.isGrounded) return;
            
            // Apply a vertical force to the character's velocity, making them jump.
            velocity.y += jumpForce;
            isJumping = true;

            return;
        }
    }

    private void HandleAnimations() 
    {
        if (isWalking)
		{
            animator.SetBool("isWalking", true);
		}
        else
        { 
            animator.SetBool("isWalking", false);
	    }

        if (isWalking && isRunning)
        {
            animator.SetBool("isRunning", true);
	    }
        else
        { 
            animator.SetBool("isRunning", false);
	    }

        if (isJumping)
        {
            animator.SetTrigger("isJumping");
	    }
        else
	    { 
            animator.ResetTrigger("isJumping");
	    }
    }
}
