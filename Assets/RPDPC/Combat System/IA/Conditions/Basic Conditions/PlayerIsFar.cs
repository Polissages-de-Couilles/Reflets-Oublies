using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerIsFar")]
public class PlayerIsFar : ConditionBase
{
    [SerializeField] bool fulfillIfTalking;
    [SerializeField] float minDistance;
    GameObject parent;
    GameObject player;
    StateManager sm;
    public override void Init(GameObject parent, GameObject player)
    {
        this.parent = parent;
        this.player = player;
        sm = this.player.GetComponent<StateManager>();
    }

    public override bool isConditionFulfilled()
    {
        if (sm.playerState == StateManager.States.talk && !fulfillIfTalking)
        {
            return false;
        }
        return Vector3.Distance(parent.transform.position, player.transform.position) > minDistance;
    }
}
