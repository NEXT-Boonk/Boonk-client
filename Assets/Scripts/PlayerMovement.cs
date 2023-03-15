using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    
    [SerializeField] private float gravity;
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float jumpForce;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float gravity;

    [SerializeField] private float sprintMultiplier = 1.4f;
    private bool isWalking; 
    private bool isRunning; 

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
        if(!IsOwner) return;
        
        // Call movement functions.
        ApplyGravity();  
        Jump();  
        Move();

        // Apply velocity.
        controller.Move(velocity * Time.deltaTime); 
        HandleAnimations();
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
<<<<<<< Updated upstream
            if (Input.GetKeyDown(KeyCode.LeftShift)) sprinting = true;
            if (Input.GetKeyUp(KeyCode.LeftShift)) sprinting = false; 

            if (sprinting) controller.Move(moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime); 
=======
            if (Input.GetButtonDown("Run")) controller.Move(moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime); 
>>>>>>> Stashed changes
            else controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime); 
        }  
            controller.Move(moveSpeed * Time.deltaTime * moveDirection.normalized);
        }
        else
        {
            isWalking = false;
        }
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
        }   
    }

    private void HandleAnimations() 
    {
        if (isWalking == true && isRunning == true)
        {
            animator.SetBool("isRunning", true);
	    }
        else if (isWalking == true)
		{
            animator.SetBool("isWalking", true);
		}
        else
        { 
            animator.SetBool("isWalking", false);
	    }
    }
}
