using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashController : MonoBehaviour
{
    private CharacterController characterController;
    private MovementController movementController;
    private PlayerInputEventManager PIE;

    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashCooldown = 1f;

    public bool isDashing { get; private set; }
    bool canDash = true;
    Vector3 gravity = new Vector3(0, -9.81f, 0);

    StateManager stateManager;
    [SerializeField] private List<StateManager.States> states = new List<StateManager.States>();

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        movementController = GetComponent<MovementController>();
        stateManager = GetComponent<StateManager>();
    }

    private void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Dash.performed += OnDash;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if(canDash && isStateCompatible(stateManager.playerState))
        {
            StartCoroutine(Dash());
            stateManager.SetPlayerState(StateManager.States.dash, dashTime);
        }
    }

    IEnumerator Dash()
    {
        canDash = false;

        isDashing = true;
        Vector3 dir = movementController.IsMovementPressed ? characterController.transform.forward : -characterController.transform.forward;
        
        for (int i = 0; i < Mathf.RoundToInt(dashTime * 50f); i++)
        {
            Vector3 dash = dir * dashForce * Time.fixedDeltaTime;
            dash += gravity * dashForce;
            characterController.Move(dash);
            yield return new WaitForFixedUpdate();
        }
        
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    bool isStateCompatible(StateManager.States state)
    {
        return states.Contains(state);
    }
}
