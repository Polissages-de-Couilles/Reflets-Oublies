using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDamageable : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    float currentHealth;

    public Action<float, float> OnDamageTaken { get; set; }
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        Debug.Log("Doll took damage. Their health is now at " + currentHealth);
        OnDamageTaken?.Invoke(damage, currentHealth);
    }

    public void heal(float heal)
    {

    }

    void Start()
    {
        currentHealth = maxHealth;
    }
}
