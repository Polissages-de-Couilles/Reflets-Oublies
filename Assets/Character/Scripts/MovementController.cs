using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MovementController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputEvent PIE;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    float rotationFactorPerFrame = 15f;

    public float Speed => speed;
    [SerializeField] private float speed = 5f;

    Vector3 gravity = new Vector3(0, -9.81f, 0);
    

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        PIE = GameObject.Find("InputManager").GetComponent<PlayerInputEvent>();
    }

    private void Start()
    {
        PIE.PlayerInputAction.Player.Movement.started += OnMovement;
        PIE.PlayerInputAction.Player.Movement.performed += OnMovement;
        PIE.PlayerInputAction.Player.Movement.canceled += OnMovement;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void HandleGravity()
    {
        bool isGrounded = characterController.isGrounded;
        
        if (!isGrounded)
        {
            characterController.Move(gravity * Time.fixedDeltaTime);
        }
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt = Quaternion.Euler(0, 45, 0) * currentMovement;
        positionToLookAt.y = 0.0f;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.fixedDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        characterController.Move(Quaternion.Euler(0, 45, 0) * currentMovement * Time.fixedDeltaTime * speed);
        HandleRotation();
        HandleGravity();
    }
}
