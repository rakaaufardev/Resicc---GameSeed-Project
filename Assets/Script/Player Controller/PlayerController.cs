using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float gravity = 1f;

    [Header("Controls")]
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;

    private CharacterController characterController;
    private bool isCharacterGrounded = false;

    private Animator animator;

    [Header("Movement Parameters")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;

    private Camera playerCamera;

    private Vector3 velocity;
    private Vector3 moveDirection;



    private void Awake()
    {
        GetReferences();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleGravity();
        HandleAnimator();
        HandleInteractionCheck();
        HandleInteractionInput();
    }

    private void GetReferences()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = moveDirection.normalized;
        moveDirection = transform.TransformDirection(moveDirection);

        characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
    }

    private void HandleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.gameObject.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);
                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        if(Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
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

