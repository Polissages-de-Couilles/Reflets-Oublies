using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class CharacterMovements : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10f;
    Rigidbody rb;
    PlayerInputEvent PIE;

    float _currentVelocity;
    Vector2 targetStickAngle;
    [SerializeField] float smoothRotaTime = 0.05f;

    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashSpeed = 10f;
    bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        PIE = GameObject.Find("InputManager").GetComponent<PlayerInputEvent>(); //To change when the InputManager will be moved in the Game Manager

        // DASH
        PIE.PlayerInputAction.Player.Dash.performed += launchDash;
        //
    }

    public void FixedUpdate()
    {
        Debug.Log(-transform.forward);
        if (!isDashing)
        {
            doRotation();
            doMovement();
        }
    }

    void doRotation()
    {
        if (PIE.PlayerInputAction.Player.Movement.ReadValue<Vector2>() != new Vector2(0, 0))
        {
            targetStickAngle = PIE.PlayerInputAction.Player.Movement.ReadValue<Vector2>();
        }
        var targetAngle = Mathf.Atan2(targetStickAngle.x, targetStickAngle.y) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothRotaTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    void doMovement()
    {
        Vector2 LerpedVelocity = Vector2.Lerp(new Vector2(rb.velocity.x, rb.velocity.z), PIE.PlayerInputAction.Player.Movement.ReadValue<Vector2>() , 1f).normalized * walkSpeed;
        rb.velocity = Quaternion.Euler(0, 45, 0) * new Vector3(LerpedVelocity.x, 0, LerpedVelocity.y);
    }

    void launchDash(InputAction.CallbackContext context)
    {
        if (!isDashing)
        {
            StartCoroutine("doDash");
        }
    }

    IEnumerator doDash() 
    {
        Debug.Log("Dash");
        isDashing = true;

        Vector2 stickDirection;
        if (PIE.PlayerInputAction.Player.Movement.ReadValue<Vector2>() != new Vector2(0, 0))
        {
            stickDirection = PIE.PlayerInputAction.Player.Movement.ReadValue<Vector2>();
        }
        else
        {
            stickDirection = new Vector2(-transform.forward.x, -transform.forward.z); //If dashing without touching stick, dash backward
        }
        rb.velocity = Quaternion.Euler(0, 45, 0) * new Vector3(stickDirection.x,0, stickDirection.y) * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector3.zero;
        isDashing = false;
    }
}
