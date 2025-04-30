using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class OnlyDoNTime : ConditionBase
{
    [SerializeField] int nTime;
    Dictionary<StateEntityBase, int> nTimeDo = new Dictionary<StateEntityBase, int>();
    StateEntityBase seb;

    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
        this.seb = seb;
        if (!nTimeDo.Keys.Contains(seb)) nTimeDo.Add(seb, 0);
    }

    public override bool isConditionFulfilled()
    {
        if (nTimeDo[seb] < nTime)
        {
            nTimeDo[seb] += 1;
            return true;
        }
        return false;
    }
}
