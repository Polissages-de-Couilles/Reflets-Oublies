using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Random True or False")]
public class RandomTrueOrFalse : ConditionBase
{
    [Range(0, 100)]
    [SerializeField] float percentageOfTrue;
    public override void Init(GameObject parent, GameObject player)
    {
    }

    public override bool isConditionFulfilled()
    {
        return Random.Range(0,100) <= percentageOfTrue;
    }
}
