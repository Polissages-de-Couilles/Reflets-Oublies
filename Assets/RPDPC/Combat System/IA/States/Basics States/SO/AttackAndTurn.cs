using ExternPropertyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SOAttack;

[CreateAssetMenu(menuName = "Game/IA/States/Base/TurnToPlayerWhileAttaking")]
public class AttackAndTurn : StateBase
{
    [SerializeField] List<AttackDetails> attacks;
    [Tooltip("True if we want the bot to continue to attack, even if the conditions are not met anymore")]
    [SerializeField] bool doAllAttacks;
    [SerializeField] Vector2 timeWithoutAttackAfter;
    [SerializeField] protected float turnDuration;

    public override StateEntityBase PrepareEntityInstance()
    {
        AttackEntityAndTurn aeat = new AttackEntityAndTurn();
        aeat.Init(attacks,doAllAttacks,timeWithoutAttackAfter,turnDuration);
        return aeat;
    }
}
