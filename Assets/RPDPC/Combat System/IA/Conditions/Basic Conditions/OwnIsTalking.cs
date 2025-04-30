using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Own/OwnIsTalking")]
public class OwnIsTalking : ConditionBase
{
    PNJ pnjComp;

    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
        pnjComp = parent.GetComponent<PNJ>();
    }

    public override bool isConditionFulfilled()
    {
        return pnjComp != null && pnjComp.state != PNJ.State.Waiting;
    }
}
