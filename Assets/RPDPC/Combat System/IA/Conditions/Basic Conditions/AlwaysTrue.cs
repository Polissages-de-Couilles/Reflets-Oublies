using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/AlwaysTrue")]
public class AlwaysTrue : ConditionBase
{
    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
    }

    public override bool isConditionFulfilled()
    {
        return true;
    }
}
