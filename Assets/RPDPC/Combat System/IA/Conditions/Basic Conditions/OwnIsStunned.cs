using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Own/OwnIsStunned")]
public class OwnIsStunned : ConditionBase
{
    GameObject parent;
    BotStunAndKnockbackManager stunAndKnockbackManager;
    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
        stunAndKnockbackManager = parent.GetComponent<BotStunAndKnockbackManager>();
    }

    public override bool isConditionFulfilled()
    {
        return stunAndKnockbackManager.Stunned;
    }
}
