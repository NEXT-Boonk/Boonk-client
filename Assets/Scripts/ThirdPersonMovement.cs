using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;

    private float turnSmoothTime = 0.1f; 
    private float turnSmoothVelocity;
    private Vector3 velocity;

    void Awake()
    {
        // hides the cursor 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ApplyGravity();  
        Jump();  
        Move();
        controller.Move(velocity * Time.deltaTime); 
    }
    
    private void ApplyGravity() 
    {
        if (!controller.isGrounded) velocity.y -= gravity * Time.deltaTime * Time.deltaTime;
        else velocity.y = -0.1f;
    }

    private void Move() 
    {
        // gets input
        float inputWS = Input.GetAxisRaw("Vertical");
        float inputAD = Input.GetAxisRaw("Horizontal");

        // normalized direction vector
        Vector3 direction = new Vector3(inputAD, 0, inputWS).normalized;
        
        // no movement if the direction vector's magnitude is too close to zero
        if (direction.magnitude >= 0.1) {
            // a trig function is used to calculate the player's look direction
            float lookDirectionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            
            // a smoothing function is used to smooth the rotation of the player model
            float playerModelAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookDirectionAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, playerModelAngle, 0f);
            
            // the movement direction is calculated using the look direction
            Vector3 moveDirection = Quaternion.Euler(0.0f, lookDirectionAngle, 0.0f) * Vector3.forward;

            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime); 
        }  
    }

    private void Jump() 
    {   
        if (Input.GetButtonDown("Jump"))
        {
            if (!controller.isGrounded) return;
            velocity.y += jumpForce;
        }   
    }
}
