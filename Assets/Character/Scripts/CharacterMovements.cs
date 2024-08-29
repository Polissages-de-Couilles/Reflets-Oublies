using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class CharacterMovements : MonoBehaviour
{
    public float speed = 100f;
    Rigidbody rb;
    Vector3 movement;
    PlayerInputEvent PIE;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        PIE = GameObject.Find("InputManager").GetComponent<PlayerInputEvent>();
    }

    public void FixedUpdate()
    {
        Vector2 mvVector = PIE.PlayerInputAction.Player.Movement.ReadValue<Vector2>();
        Vector2 LerpedVelocity = Vector2.Lerp(new Vector2(rb.velocity.x, rb.velocity.z), mvVector, 1f).normalized * speed;
        rb.velocity = Quaternion.Euler(0, 45, 0) * new Vector3(LerpedVelocity.x, 0, LerpedVelocity.y);
        Debug.Log(LerpedVelocity);
    }
}
