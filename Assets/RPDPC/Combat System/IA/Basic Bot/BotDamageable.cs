using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDamageable : MonoBehaviour, IDamageable
{
    GuardManager gm;
    public float maxHealth = 3f;
    float currentHealth;

    public Action<float, float> OnDamageTaken { get; set; }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void takeDamage(float damage, GameObject attacker)
    {
        if (gm.isGuarding != true)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }
            Debug.Log("Bot took damage. Their health is now at " + currentHealth);
            OnDamageTaken?.Invoke(damage, currentHealth);
        }
        else
        {
            gm.ApplyGuard(attacker);
        }
    }

    public void heal(float heal)
    {

    }

    void Start()
    {
        currentHealth = maxHealth;
        gm = GetComponent<GuardManager>();
    }
}
