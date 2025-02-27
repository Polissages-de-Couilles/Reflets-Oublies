using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashController : MonoBehaviour
{
    private CharacterController characterController;
    private MovementController movementController;
    private PlayerDamageable playerDamageable;
    private PlayerInputEventManager PIE;

    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashCooldown = 2f;

    public bool isDashing { get; private set; }
    bool canDash = true;
    Vector3 gravity = new Vector3(0, -9.81f, 0);

    StateManager stateManager;
    [SerializeField] private List<StateManager.States> states = new List<StateManager.States>();

    public int DashMax => dashMax;
    private int dashMax = 3;
    public int DashCount => dashCount;
    private int dashCount;

    private bool isRecharging = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        movementController = GetComponent<MovementController>();
        stateManager = GetComponent<StateManager>();
        playerDamageable = GetComponent<PlayerDamageable>();
    }

    private void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Dash.performed += OnDash;
        StartCoroutine(RechargeDash());
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if(canDash && isStateCompatible(stateManager.playerState) && dashCount > 0)
        {
            StartCoroutine(Dash());
            stateManager.SetPlayerState(StateManager.States.dash, dashTime);
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        dashCount--;
        isDashing = true;

        playerDamageable.BecameInvicible(dashTime);

        if (GameManager.Instance.UIManager != null)
        {
            GameManager.Instance.UIManager.UpdateDashSlider(-1f);
        }

        Vector3 dir = movementController.IsMovementPressed ? Quaternion.Euler(0, -45, 0) * movementController.Direction.normalized : -characterController.transform.forward;


        for (int i = 0; i < Mathf.RoundToInt(dashTime * 50f); i++)
        {
            Vector3 dash = dir * dashForce * Time.fixedDeltaTime;
            dash += gravity * dashForce;
            RaycastHit hit;
            RaycastHit hit2;
            if (!Physics.Raycast(transform.position + (transform.forward * 0.5f), Vector3.down, out hit, 2f) || !Physics.Raycast(transform.position + (-transform.forward * 0.5f), Vector3.down, out hit2, 2f))
            {
                characterController.Move(new Vector3(0, 0, 0));
                break;
            }
            else if(!stateManager.playerState.Equals(StateManager.States.talk))
            {
                characterController.Move(dash);
            }
            yield return new WaitForFixedUpdate();
        }
        movementController.Acceleration = 1f;

        isDashing = false;
        canDash = true;
    }

    IEnumerator RechargeDash()
    {
        while (true)
        {
            if(dashCount < dashMax)
            {
                
                for (int i = 0; i < Mathf.RoundToInt(dashCooldown * 50f); i++)
                {
                    yield return new WaitForFixedUpdate();
                    if(i > Mathf.RoundToInt(dashCooldown * 25f))
                    {
                        isRecharging = true;
                        if (GameManager.Instance.UIManager != null)
                        {
                            GameManager.Instance.UIManager.UpdateDashSlider(1f/Mathf.RoundToInt(dashCooldown * 25f));
                        }
                    }
                }

                //yield return new WaitForSeconds(dashCooldown);
                dashCount++;
                isRecharging = false;
                GameManager.Instance.UIManager.SetDashSlider(dashCount);
            }
            else
                yield return null;
        }
    }

    bool isStateCompatible(StateManager.States state)
    {
        return states.Contains(state);
    }

    public void SimulateDash()
    {
        if (canDash && dashCount > 0)
        {
            StartCoroutine(Dash());
            stateManager.SetPlayerState(StateManager.States.dash, dashTime);
        }
    }
}
