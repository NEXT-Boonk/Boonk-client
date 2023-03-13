using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private float gravity;
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float jumpForce;

    private readonly float turnSmoothTime = 0.1f; 
    private float turnSmoothVelocity;
    private Vector3 velocity;


    void Awake() {
        // Hides the cursor 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        if(!IsOwner) return;
        ApplyGravity();  
        Jump();  
        Move();
        controller.Move(velocity * Time.deltaTime);
    }
    
    private void ApplyGravity() {
        if (!controller.isGrounded) velocity.y -= gravity * Time.deltaTime;
        else velocity.y = -1f;
    }

    private void Move() {
        // Reads input.
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");

        // Normalized direction vector, to move the player.
        Vector3 direction = new Vector3(inputHorizontal, 0, inputVertical).normalized;
        
        // Don't move, if the direction vector's magnitude is too close to zero.
        if (direction.magnitude >= 0.1) {
            // Calculate the player's look direction.
            float lookDirectionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            
            // Smooth the rotation of the player model.
            float playerAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookDirectionAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, playerAngle, 0f);
            
            // The movement direction is calculated using the look direction.
            Vector3 moveDirection = Quaternion.Euler(0.0f, lookDirectionAngle, 0.0f) * Vector3.forward;

            controller.Move(moveSpeed * Time.deltaTime * moveDirection.normalized); 
        }  
    }

    private void Jump() {
        if (Input.GetButtonDown("Jump")) {
            if (!controller.isGrounded) return;
            velocity.y += jumpForce;
        }
    }
}
