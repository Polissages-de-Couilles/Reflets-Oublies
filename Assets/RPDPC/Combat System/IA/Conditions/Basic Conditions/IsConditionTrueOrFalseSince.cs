using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/IsConditionTrueOrFalseSince")]
public class IsConditionTrueOrFalseSince : ConditionBase
{
    [SerializeField] ConditionBase condition;
    [SerializeField] bool checkForTrue;
    [SerializeField] float sinceSeconds;

    float timer;
    float lastUpdate;

    public override void Init(GameObject parent, GameObject player)
    {
        
    }

    public override bool isConditionFulfilled()
    {
        if (Time.time - lastUpdate > 0.5 || checkForTrue != condition.isConditionFulfilled())
        {
            timer = 0;
        }

        lastUpdate = Time.time;
        timer += Time.deltaTime; 

        return timer > sinceSeconds;
    }
}
