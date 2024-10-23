using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    float currentHealth;

    private PlayerInputEventManager PIE;

    public Action<float, float> OnDamageTaken { get; set; }
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        Debug.Log("Player took damage. Their health is now at " + currentHealth);
        OnDamageTaken?.Invoke(damage, currentHealth);
    }

    void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Potion.performed += Heal;
        currentHealth = maxHealth;
        //StartCoroutine(testDamage());
    }

    private void Heal(InputAction.CallbackContext context)
    {
        print("lol je Heal");
    }

    IEnumerator testDamage() { yield return new WaitForSeconds(2); takeDamage(maxHealth); }
}
