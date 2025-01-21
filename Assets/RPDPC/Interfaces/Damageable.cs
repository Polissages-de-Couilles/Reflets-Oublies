using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public Action<float, float> OnDamageTaken { get; set; }
    void takeDamage(float damage);
    public float getCurrentHealth();
    public float getMaxHealth();
    void heal(float heal);
}
