using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/AlwaysFalse")]
public class AlwaysFalse : ConditionBase
{
    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
    }

    public override bool isConditionFulfilled()
    {
        return false;
    }
}
