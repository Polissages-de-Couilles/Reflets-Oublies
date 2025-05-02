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
    private AnimationManager animationManager;

    Vector2 currentMovementInput;
    public Vector3 Direction => currentMovement;
    Vector3 currentMovement;
    public bool IsMovementPressed => isMovementPressed;
    bool isMovementPressed;
    float rotationFactorPerFrame = 15f;

    public float Speed => speed;
    [SerializeField] private float speed = 5f;
    public float Acceleration { get; set; }
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
        animationManager = GetComponent<AnimationManager>();
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
        if(GameManager.Instance.LockManager.CurrentLockObject != null)
        {
            Vector3 positionToLookAt = (GameManager.Instance.LockManager.CurrentLockObject.transform.position - this.transform.position);
            positionToLookAt.y = 0.0f;

            Quaternion currentRotation = transform.rotation;

            if (isStateCompatible(stateManager.playerState))
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.fixedDeltaTime);
            }
        }
        else
        {
            Vector3 positionToLookAt = Quaternion.Euler(0, -45, 0) * currentMovement;
            positionToLookAt.y = 0.0f;

            Quaternion currentRotation = transform.rotation;

            if (isMovementPressed && isStateCompatible(stateManager.playerState))
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.fixedDeltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        HandleRotation();
        if (isStateCompatible(stateManager.playerState))
        {
            RaycastHit hit;
            if(!Physics.Raycast(transform.position + (transform.forward * 0.5f), Vector3.down, out hit, 2f))
            {
                characterController.Move(new Vector3(0, 0, 0));
                Velocity = new Vector3(0, 0, 0);
            }
            else
            {
                var s = Mathf.Lerp(0, speed, Mathf.Clamp01(Acceleration));
                if (IsMovementPressed)
                {
                    Acceleration += 5f * Time.fixedDeltaTime;
                    Acceleration = Mathf.Clamp01(Acceleration);
                }
                else
                {
                    Acceleration -= 10f * Time.fixedDeltaTime;
                    Acceleration = Mathf.Clamp01(Acceleration);
                }

                characterController.Move(Quaternion.Euler(0, -45, 0) * currentMovement * Time.fixedDeltaTime * s);
                Velocity = currentMovement * speed;
            }
        }
        else
        {
            characterController.Move(new Vector3(0,0,0));
            Velocity = new Vector3(0, 0, 0);
        }
        HandleGravity();
        oldPosition = currentPosition;
        currentPosition = transform.position;
        animationManager.SetSpeed(Velocity.normalized.sqrMagnitude);
    }

    bool isStateCompatible(StateManager.States state)
    {
        return states.Contains(state);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position + (transform.forward * 0.5f), transform.position + (transform.forward * 0.5f) + (Vector3.down * 2f));
    }
}
