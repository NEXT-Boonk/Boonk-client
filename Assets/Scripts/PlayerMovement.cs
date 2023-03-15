using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float jumpForce;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float gravity;

    bool sprinting = false;

    private float turnSmoothTime = 0.1f; 
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
        
        // No movement if the direction vector's magnitude is too close to zero.
        if (direction.magnitude >= 0.1) {
            // A trig function is used to calculate the player's look direction.
            float lookDirectionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            
            // A smoothing function is used to smooth the rotation of the player model.
            float playerModelAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookDirectionAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, playerModelAngle, 0f);
            
            // Calculate the movement direction using the look direction.
            Vector3 moveDirection = Quaternion.Euler(0.0f, lookDirectionAngle, 0.0f) * Vector3.forward;

            // Apply movement to the player.
            if (Input.GetKeyDown(KeyCode.LeftShift)) sprinting = true;
            if (Input.GetKeyUp(KeyCode.LeftShift)) sprinting = false; 

            if (sprinting) controller.Move(moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime); 
            else controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime); 
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
}
