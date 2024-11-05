using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/doNothing")]
public class doNothing : StateBase
{
    public override void Init(StateMachineManager manager, GameObject parent, GameObject player)
    {
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
