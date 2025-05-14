using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/OnlyDoNTime")]
public class OnlyDoNTime : ConditionBase
{
    [SerializeField] int nTime;
    StateEntityBase seb;
    GameObject parent;

    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
        this.seb = seb;
        this.parent = parent;
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
