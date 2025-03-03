using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsConditionTrueOrFalseSince : ConditionBase
{
    [SerializeField] int ConditonID;
    StateEntityBase state;

    public override void Init(GameObject parent, GameObject player, StateEntityBase state)
    {
        this.state = state;
    }

    public override bool isConditionFulfilled()
    {
        throw new System.NotImplementedException();
    }
}
