using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionBase : ScriptableObject
{
    public virtual void Init(GameObject parent, GameObject player)
    {

    }

    public virtual void Init(GameObject parent, GameObject player, StateEntityBase state)
    {

    }

    public abstract bool isConditionFulfilled();
}
