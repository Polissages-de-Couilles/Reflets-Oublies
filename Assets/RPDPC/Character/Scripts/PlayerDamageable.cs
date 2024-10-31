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
    public float currentHealth => _currentHealth;
    private float _currentHealth;
    public Action<float, float> OnDamageTaken { get; set; }
    public void takeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
        }
        Debug.Log("Player took damage. Their health is now at " + _currentHealth);
        OnDamageTaken?.Invoke(damage, _currentHealth);
    }

    void Start()
    {
        _currentHealth = maxHealth;
        //StartCoroutine(testDamage());
    }

    public void heal(float heal)
    {
        _currentHealth += heal;
        if (maxHealth < _currentHealth) _currentHealth = maxHealth;
    }

    IEnumerator testDamage() { yield return new WaitForSeconds(2); takeDamage(maxHealth); }
}
