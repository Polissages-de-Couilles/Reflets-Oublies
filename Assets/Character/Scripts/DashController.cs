using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputEvent PIE;

    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashCooldown = 1f;

    public bool isDashing { get; private set; }
    bool canDash = true;
    Vector3 gravity = new Vector3(0, -9.81f, 0);

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        PIE = GameObject.Find("InputManager").GetComponent<PlayerInputEvent>();
    }

    private void Start()
    {
        PIE.PlayerInputAction.Player.Dash.performed += OnDash;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if(canDash)
            StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        canDash = false;

        isDashing = true;
        for (int i = 0; i < Mathf.RoundToInt(dashTime * 50f); i++)
        {
            Vector3 dash = characterController.transform.forward * dashForce * Time.fixedDeltaTime;
            dash += gravity * dashForce;
            characterController.Move(dash);
            yield return new WaitForFixedUpdate();
        }
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
