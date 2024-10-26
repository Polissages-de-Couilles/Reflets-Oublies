using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/FollowObject")]
public class FollowObject : StateBase
{
    public GameObject player;
    GameObject parent;

    public override void Init(StateMachineManager manager, GameObject parent, GameObject player)
    {
        this.parent = parent;
        this.player = player;
    }

    public override void ExitState()
    {
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
    }

    public override void OnUpdate()
    {
    }
}
