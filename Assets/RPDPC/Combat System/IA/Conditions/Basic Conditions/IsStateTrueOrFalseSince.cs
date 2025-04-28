using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/IsStateTrueOrFalseSince")]
public class IsStateTrueOrFalseSince : ConditionBase
{
    [SerializeField] StateBase state;
    [SerializeField] bool checkForTrue;
    [SerializeField] float sinceSeconds;

    StateEntityBase entity;
    float timer;
    float lastUpdate;

    public override void Init(GameObject parent, GameObject player)
    {
        entity = state.PrepareEntityInstance();
    }

    public override bool isConditionFulfilled()
    {
        if (Time.time - lastUpdate > 0.5 || checkForTrue != entity.isStateValid())
        {
            timer = 0;
        }

        lastUpdate = Time.time;
        timer += Time.deltaTime;

        return timer > sinceSeconds;
    }
}
