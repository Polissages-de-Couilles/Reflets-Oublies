using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StunAndKnockbackManagerBase : MonoBehaviour
{
    protected Vector3 gravity = new Vector3(0, -9.81f, 0);
    protected float knockbackDuration = 0.07f;

    public abstract void ApplyStun(float stunDuration);
    public abstract void ApplyKnockback(float knockbackForce, KnockbackMode mode, GameObject attacker, GameObject attacked, Vector3 collisionPosWhenTouched);

    protected abstract IEnumerator ApplyKnockback(Vector3 finalPos, Vector3 attackedPos);
}

[Serializable]
public enum KnockbackMode
{
    MoveAwayFromAttacker,
    MoveAwayFromAttackCollision,
}
