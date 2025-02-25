using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    public Action asGuarded;
    public bool isGuarding;
    public float selfKnockbackForce;
    public float opponentKnockbackForce;
    public float opponentStunDuration;

    public void ApplyGuard(GameObject attacker)
    {
        PlayerStunAndKnockbackManager psakm = attacker.GetComponent<PlayerStunAndKnockbackManager>();
        psakm.ApplyKnockback(opponentKnockbackForce, KnockbackMode.MoveAwayFromAttacker, gameObject, attacker, Vector3.zero);
        GetComponent<BotStunAndKnockbackManager>().ApplyKnockback(selfKnockbackForce, KnockbackMode.MoveAwayFromAttacker, attacker, gameObject, Vector3.zero);
        if (opponentStunDuration != 0)
        {
            psakm.ApplyStun(opponentStunDuration);
        }
        asGuarded?.Invoke();
    }
}
