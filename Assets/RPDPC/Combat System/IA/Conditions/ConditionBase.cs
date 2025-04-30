using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionBase : ScriptableObject
{
    public virtual void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {

    }

    public abstract bool isConditionFulfilled();
}
