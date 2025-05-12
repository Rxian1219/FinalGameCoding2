using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


    [Header("Movement")]
    public float walkSpeed = 5f;                
    public float runMultiplier = 1.5f;          
    public float crouchSpeedMultiplier = 0.5f;    
    public float jumpForce = 3.5f;                
    public float gravity = -20f;                 
    public float mouseSensitivity = 2f;           

    [Header("Stamina")]
    public float stamina = 10f;                  
    public float maxStamina = 10f;               
    public float runStaminaCostPerHalfSecond = 1f; 
    public float jumpStaminaCost = 1f;            
    public float staminaRegenDelay = 1.5f;        
    public float staminaRegenRate = 2f;          
    public Image staminaBar;              


    [Header("Crouch")]
    public float crouchScale = 0.66f;
    public bool isCrouching { get; private set; }


    [Header("Jump Buffer")]
    public float jumpGraceTime = 0.2f;         


    private CharacterController controller;     
    private Transform cameraTransform;           
    private float yRotation = 0f;          
    private float originalYScale;                 
    private float staminaUseTimer = 0f;        
    private float staminaRegenTimer = 0f;      
    private float lastGroundedTime;           
    private float lastJumpPressedTime;       
    private bool isRunning = false;        
    private Vector3 velocity;            

    void Start()
    {
        controller = GetComponent<CharacterController>();      
        cameraTransform = Camera.main.transform;              
        Cursor.lockState = CursorLockMode.Locked;              
        Cursor.visible = false;                             
        originalYScale = transform.localScale.y;              
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }

        HandleLook();       
        HandleMovement();   
        HandleJump();       
        UpdateStaminaBar();  
    }

   
    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX); 

        yRotation -= mouseY; 
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }

   
    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;


        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = move.magnitude > 0.1f;

        float speed = walkSpeed;

       
        if (isCrouching)
        {
            speed *= crouchSpeedMultiplier;
        }

       
        isRunning = wantsToRun && !isCrouching && isMoving && stamina > 0f;

        if (isRunning)
        {
            speed *= runMultiplier;
        }

      
        controller.Move(move * speed * Time.deltaTime);

       

        if (isRunning)
        {
            staminaUseTimer += Time.deltaTime;
            if (staminaUseTimer >= 0.5f)
            {
                stamina = Mathf.Max(0f, stamina - runStaminaCostPerHalfSecond);
                staminaUseTimer = 0f;
            }
            staminaRegenTimer = 0f; 
        }
        else
        {
           
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer >= staminaRegenDelay)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
            }
        }

       
        if (isCrouching)
        {
            transform.localScale = new Vector3(1f, originalYScale * crouchScale, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, originalYScale, 1f);
        }
    }

    
    void HandleJump()
    {
        
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

       
        if (controller.isGrounded)
            lastGroundedTime = Time.time;

       
        if (Input.GetKeyDown(KeyCode.Space))
            lastJumpPressedTime = Time.time;

        
        if (Time.time - lastGroundedTime <= jumpGraceTime &&
            Time.time - lastJumpPressedTime <= jumpGraceTime &&
            stamina >= jumpStaminaCost)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); 
            stamina -= jumpStaminaCost; 

           
            lastGroundedTime = -999f;
            lastJumpPressedTime = -999f;
        }

        
        if (!controller.isGrounded && velocity.y < 0)
        {
            velocity.y += gravity * 1.5f * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    
    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }

}
