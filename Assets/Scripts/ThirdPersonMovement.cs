using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float gravity = 9.8f;

    [SerializeField] public Transform cameraTransform;
    [SerializeField] private float moveSpeed = 10.0f; 
    [SerializeField] private float jumpForce = 5.0f;

    private readonly float turnSmoothTime = 0.1f; 
    private float turnSmoothVelocity;
    private Vector3 velocity;


    void Awake() {
        // Hides the cursor 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = GetComponentInChildren<Transform>();
    }

    void Update() {
        //if(!IsOwner) return;
        ApplyGravity();  
        Jump();  
        Move();
    }
    
    private void ApplyGravity() 
    {
        if (!controller.isGrounded) velocity.y -= gravity * Time.deltaTime * Time.deltaTime;
        else velocity.y = -0.1f;
    }

    private void Move() {
        // Reads input.
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");

        // Normalized direction vector, to move the player.
        Vector3 direction = new Vector3(-inputHorizontal, 0, -inputVertical).normalized;
        
        // Don't move, if the direction vector's magnitude is too close to zero.
        if (direction.magnitude >= 0.1) {
            // Calculate the player's look direction.
            float lookDirectionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            
            // Smooth the rotation of the player model.
            float playerAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookDirectionAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, playerAngle, 0f);
            
            // The movement direction is calculated using the look direction.
            Vector3 moveDirection = -(Quaternion.Euler(0.0f, lookDirectionAngle, 0.0f) * Vector3.forward);

            controller.Move(moveSpeed * Time.deltaTime * moveDirection.normalized); 
        }  
    }

    private void Jump() {
        if (Input.GetButtonDown("Jump"))
        {
            if (!controller.isGrounded) return;
            velocity.y += jumpForce;
        }
    }
}
