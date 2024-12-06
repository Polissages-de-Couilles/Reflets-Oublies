using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float currentHealth;
    private float defence = 1f;
    StateManager sm;

    public Action<float, float> OnDamageTaken { get; set; }

    List<StateManager.States> incompatibleStates = new List<StateManager.States> { StateManager.States.talk };

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void ChangeDefence(float _defenceChange)
    {
        defence = defence * (1 + _defenceChange);
    }

    public void takeDamage(float damage)
    {
        if (!incompatibleStates.Contains(sm.playerState))
        {
            currentHealth -= damage / defence;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }
            Debug.Log("Player took damage. Their health is now at " + currentHealth);
            OnDamageTaken?.Invoke(damage, currentHealth);
        }
    }

    void Awake()
    {
        currentHealth = maxHealth;
        //StartCoroutine(testDamage());
        sm = GetComponent<StateManager>();
    }

    public void heal(float heal)
    {
        currentHealth += heal;
        if (maxHealth < currentHealth) currentHealth = maxHealth;
    }

    IEnumerator testDamage() { yield return new WaitForSeconds(2); takeDamage(maxHealth); }
}
