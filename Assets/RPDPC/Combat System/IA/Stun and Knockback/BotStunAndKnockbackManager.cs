using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotStunAndKnockbackManager : StunAndKnockbackManagerBase
{
    Rigidbody rb;

    [HideInInspector] public bool Stunned = false;
    [SerializeField] bool canBeStunned = true;

    public override void ApplyKnockback(float knockbackForce, KnockbackMode mode, GameObject attacker, GameObject attacked, Vector3 collisionPosWhenTouched)
    {
        rb = GetComponent<Rigidbody>();
        Vector3 finalPos = new Vector3();
        switch (mode)
        {
            case KnockbackMode.MoveAwayFromAttackCollision:
                finalPos = attacked.transform.position + (collisionPosWhenTouched - attacked.transform.position).normalized * knockbackForce;
                break;
            case KnockbackMode.MoveAwayFromAttacker:
                finalPos = attacked.transform.position - (attacker.transform.position - attacked.transform.position).normalized * knockbackForce;
                break;
        }
        Debug.Log(gameObject + " : Knockback from : " + attacked.transform.position + " to " + finalPos);
        StartCoroutine(ApplyKnockback(finalPos, attacked.transform.position));
    }

    public override void ApplyStun(float stunDuration)
    {
        if (canBeStunned)
        {
            StopAllCoroutines();
            Stunned = true;
            StartCoroutine(cancelStun(stunDuration));
        }
    }

    protected override IEnumerator ApplyKnockback(Vector3 finalPos, Vector3 attackedPos)
    {
        float time = 0;
        while (time < knockbackDuration)
        {
            Vector3 knockback = Vector3.Lerp(Vector3.zero, finalPos - attackedPos, time / knockbackDuration);
            //knockback += gravity;
            knockback.y = 0;
            rb.MovePosition(attackedPos + knockback);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator cancelStun(float duration) 
    { 
        yield return new WaitForSeconds(duration); 
        Stunned = false; 
    }
}
