using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StunAndKnockbackManagerBase : MonoBehaviour
{
    public abstract void ApplyStun(float stunDuration);
    public void ApplyKnockback(float knockbackForce, KnockbackMode mode, GameObject attacker, GameObject attacked, Vector3 collisionPosWhenTouched)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        Vector3 forceToAdd = new Vector3();
        switch (mode) {
            case KnockbackMode.MoveAwayFromAttackCollision:
                forceToAdd = (collisionPosWhenTouched - attacked.transform.position).normalized * knockbackForce;
                break;
            case KnockbackMode.MoveAwayFromAttacker:
                forceToAdd = -(attacker.transform.position - attacked.transform.position).normalized * knockbackForce;
                break;
        }
        body.velocity = forceToAdd;
    }
}

[Serializable]
public enum KnockbackMode
{
    MoveAwayFromAttacker,
    MoveAwayFromAttackCollision,
}
