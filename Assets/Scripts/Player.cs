using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform body;
    [SerializeField] private CharacterController controller;

    // Movements settings
    [SerializeField] private int speed = 10;
    [SerializeField] private float g = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float sensitivity = 400;

    // Rotation around x-axis in degrees
    private float rotationX;

    // Velocity
    private Vector3 velocity = new Vector3(0, 0, 0);

    void Awake()
    {
        // Mouse settings
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Camera();
        Movement();
    }

    private void Camera() 
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        head.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
    }

    int count = 0;

    private void Movement() 
    {   
        // Jumping      
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            print("shit");
            velocity.y += Mathf.Sqrt(jumpHeight * -2.0f * g);
        }

        // Applying gravity
        velocity.y += g * Time.deltaTime;
        // Ground check
        if (controller.isGrounded && velocity.y < 0) velocity.y = 0f;
        controller.Move(velocity * Time.deltaTime);

        // Walking
        Vector3 move = body.forward * Input.GetAxisRaw("Vertical") + body.right * Input.GetAxisRaw("Horizontal");
        controller.Move(move.normalized * speed * Time.deltaTime);             
    }
}

