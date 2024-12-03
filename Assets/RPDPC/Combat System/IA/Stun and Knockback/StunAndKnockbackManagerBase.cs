using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class StunAndKnockbackManagerBase : MonoBehaviour
{
    CharacterController characterController;
    Vector3 gravity = new Vector3(0, -9.81f, 0);
    float knockbackDuration = 0.07f;

    public abstract void ApplyStun(float stunDuration);
    public void ApplyKnockback(float knockbackForce, KnockbackMode mode, GameObject attacker, GameObject attacked, Vector3 collisionPosWhenTouched)
    {
        characterController = GetComponent<CharacterController>();
        Vector3 finalPos = new Vector3();
        switch (mode) {
            case KnockbackMode.MoveAwayFromAttackCollision:
                finalPos = attacked.transform.position + (collisionPosWhenTouched - attacked.transform.position).normalized * knockbackForce;
                break;
            case KnockbackMode.MoveAwayFromAttacker:
                finalPos = attacked.transform.position - (attacker.transform.position - attacked.transform.position).normalized * knockbackForce;
                break;
        }
        Debug.Log("Knockback from : " + attacked.transform.position + " to " + finalPos);
        StartCoroutine(ApplyKnockback(finalPos, attacked.transform.position));
    }

    IEnumerator ApplyKnockback(Vector3 finalPos, Vector3 attackedPos)
    {
        float time = 0;
        while (time < knockbackDuration) {
            Vector3 knockback = Vector3.Lerp(Vector3.zero, finalPos - attackedPos, time/ knockbackDuration);
            knockback += gravity;
            characterController.Move(knockback);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}

[Serializable]
public enum KnockbackMode
{
    MoveAwayFromAttacker,
    MoveAwayFromAttackCollision,
}
