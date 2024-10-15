using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionBase : ScriptableObject
{
    public abstract bool Init(GameObject parent, GameObject player);

    public abstract bool isConditionFulfilled();
}
