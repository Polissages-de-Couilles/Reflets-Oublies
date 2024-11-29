using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MovementController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputEventManager PIE;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    public bool IsMovementPressed => isMovementPressed;
    bool isMovementPressed;
    float rotationFactorPerFrame = 15f;

    public float Speed => speed;
    [SerializeField] private float speed = 5f;
    public Vector3 Velocity { get; private set; }
    Vector3 oldPosition, currentPosition;
    public float DistanceTravelInOneFrame => (currentPosition - oldPosition).magnitude;

    Vector3 gravity = new Vector3(0, -9.81f, 0);
    
    StateManager stateManager; 
    [SerializeField] private List<StateManager.States> states = new List<StateManager.States>();

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        stateManager = GetComponent<StateManager>();
    }

    private void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Movement.started += OnMovement;
        PIE.PlayerInputAction.Player.Movement.performed += OnMovement;
        PIE.PlayerInputAction.Player.Movement.canceled += OnMovement;
        oldPosition = currentPosition = transform.position;
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

        if (isMovementPressed && isStateCompatible(stateManager.playerState))
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.fixedDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (isStateCompatible(stateManager.playerState))
        {
            characterController.Move(Quaternion.Euler(0, 45, 0) * currentMovement * Time.fixedDeltaTime * speed);
            Velocity = currentMovement * speed;
        }
        else
        {
            characterController.Move(new Vector3(0,0,0));
            Velocity = new Vector3(0, 0, 0);
        }
        HandleRotation();
        HandleGravity();
        oldPosition = currentPosition;
        currentPosition = transform.position;
    }

    bool isStateCompatible(StateManager.States state)
    {
        return states.Contains(state);
    }
}
