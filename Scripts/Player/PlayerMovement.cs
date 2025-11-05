using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController characterController;

    public float speed = 12f;

    private float gravity = -9.81f;


    public Transform groundCheck;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;

    bool isGrounded;

    Vector3 velocity;

    public float jumpHeight = 3;


    public bool isSprinting;

    public float sprintingSpeedMultiplier = 1.5f;

    private float sprintSpeed = 1;

    public float staminaUseAmount = 5;

    private StaminaBar staminaBar;

    public Animator animator;

    private void Awake()
    {
        staminaBar = FindObjectOfType<StaminaBar>();
        if (staminaBar == null)
        {
            Debug.LogWarning($"{nameof(PlayerMovement)}: No {nameof(StaminaBar)} found. Sprinting will not consume stamina.");
        }
    }


    void Update()
    {


        isGrounded = Physics.CheckSphere(groundCheck.position,sphereRadius,groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
       
        float x = Input.GetAxis("Horizontal");

        float z = Input.GetAxis("Vertical");

        animator.SetFloat("VelX",x);
        animator.SetFloat("VelZ",z);
        animator.SetBool("isSprinting",isSprinting);

        Vector3 move = transform.right * x + transform.forward * z;

        JumpCheck();
   
        RunCheck();

        characterController.Move(move * speed * Time.deltaTime*sprintSpeed);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        UpdateJumpAnimationState();
    }

    public void JumpCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

            animator.SetBool("isJumping", true);
        }

    }

    public void RunCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = !isSprinting;

            if (isSprinting)
            {
                staminaBar?.SpendStart(staminaUseAmount);
            }
            else
            {
                staminaBar?.SpendStop();
            }
        }

        if (isSprinting)
        {
            sprintSpeed = sprintingSpeedMultiplier;
            if (staminaBar != null && staminaBar.IsDepleted)
            {
                ForceStopSprinting();
            }
        }
        else
        {
            sprintSpeed = 1f;
        }
    }

    public void ForceStopSprinting()
    {
        if (!isSprinting)
        {
            return;
        }

        isSprinting = false;
        sprintSpeed = 1f;
    }

    private void UpdateJumpAnimationState()
    {
        if (animator == null)
        {
            return;
        }

        if (isGrounded && Mathf.Abs(velocity.y) <= 0.01f)
        {
            animator.SetBool("isJumping", false);
        }
    }
}
