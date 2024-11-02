using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Action<float, float> OnDamageTaken { get; set; }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

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
        currentHealth = maxHealth;
        //StartCoroutine(testDamage());
    }

    IEnumerator testDamage() { yield return new WaitForSeconds(2); takeDamage(maxHealth); }
}
