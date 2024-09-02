using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class CharacterMovements : MonoBehaviour
{
    public float speed = 100f;
    Rigidbody rb;
    PlayerInputEvent PIE;

    float _currentVelocity;
    Vector2 targetStickAngle;
    [SerializeField] float smoothRotaTime = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        PIE = GameObject.Find("InputManager").GetComponent<PlayerInputEvent>(); //To change when the InputManager will be moved in the Game Manager
    }

    public void FixedUpdate()
    {
        doRotation();
        doMovement();
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
        Vector2 LerpedVelocity = Vector2.Lerp(new Vector2(rb.velocity.x, rb.velocity.z), PIE.PlayerInputAction.Player.Movement.ReadValue<Vector2>() , 1f).normalized * speed;
        rb.velocity = Quaternion.Euler(0, 45, 0) * new Vector3(LerpedVelocity.x, 0, LerpedVelocity.y);
    }
}
