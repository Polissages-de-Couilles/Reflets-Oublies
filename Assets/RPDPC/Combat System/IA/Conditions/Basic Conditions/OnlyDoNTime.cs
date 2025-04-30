using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class OnlyDoNTime : ConditionBase
{
    [SerializeField] int nTime;
    StateEntityBase seb;

    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
        this.seb = seb;
    }

    public override bool isConditionFulfilled()
    {
        if (seb.GetStateManager().getNbTimeStateWasAchieved(seb) < nTime)
        {
            return true;
        }
        return false;
    }
}
