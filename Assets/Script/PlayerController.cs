using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float gravity = 30f;

    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    private CharacterController characterController;
    private bool isCharacterGrounded = false;

    private Animator animator;

    private Vector3 velocity;
    private Vector3 moveDirection;

    private void Awake()
    {
        GetReferences();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleIsGrounded();
        HandleGravity();
        HandleAnimator();
    }

    private void GetReferences()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = moveDirection.normalized;

        characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
    }

    private void HandleIsGrounded()
    {
        isCharacterGrounded = Physics.CheckSphere(transform.position, groundDistance);
    }

    private void HandleGravity()
    {
        if (isCharacterGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime; 
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleAnimator()
    {
        Vector3 velocity = characterController.velocity;
        float speed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        animator.SetFloat("Speed", speed);
    }
}
