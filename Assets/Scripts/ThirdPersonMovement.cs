using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 10.0f; 
    [SerializeField] private float turnSmoothTime = 0.1f; 
    [SerializeField] private float jumpHeight = 5.0f;

    private float turnSmoothVelocity;
    private float g = 9.81f;


    void Awake()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    { 
        float inputWS = Input.GetAxisRaw("Vertical");
        float inputAD = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(inputAD, 0, inputWS).normalized;

        if (direction.magnitude >= 0.1) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

            // Applying gravity
            if (!controller.isGrounded) moveDir.y += g * Time.deltaTime;
            else if (direction.y < 0) direction.y = 0f;

            controller.Move(moveDir * speed * Time.deltaTime); 
        }
        
        /*
        // Jumping      
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            print("shit");
            direction.y += Mathf.Sqrt(jumpHeight * -2.0f * g);
        }  
        */    
    }
}
