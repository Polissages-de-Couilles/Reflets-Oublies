using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTrueOrFalse : ConditionBase
{
    [Tooltip("Value between 0 and 1")]
    [SerializeField] float percentageOfTrue;
    public override void Init(GameObject parent, GameObject player)
    {
    }

    public override bool isConditionFulfilled()
    {
        return Random.Range(0,1) <= percentageOfTrue;
    }
    void OnValidate()
    {
        percentageOfTrue = Mathf.Clamp(percentageOfTrue, 0, 1);
    }
}
