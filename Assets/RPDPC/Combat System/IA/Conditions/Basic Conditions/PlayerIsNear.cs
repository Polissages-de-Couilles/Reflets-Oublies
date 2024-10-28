using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerIsNear")]
public class PlayerIsNear : ConditionBase
{
    [SerializeField] float minDistance;
    GameObject parent;
    GameObject player;
    public override void Init(GameObject parent, GameObject player)
    {
        this.parent = parent;
        this.player = player;
    }

    public override bool isConditionFulfilled()
    {
        return Vector3.Distance(parent.transform.position, player.transform.position) <= minDistance;
    }
}
