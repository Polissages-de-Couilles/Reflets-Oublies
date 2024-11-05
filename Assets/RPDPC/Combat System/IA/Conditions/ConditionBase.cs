using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionBase : ScriptableObject
{
    public abstract void Init(GameObject parent, GameObject player);

    public abstract bool isConditionFulfilled();
}
