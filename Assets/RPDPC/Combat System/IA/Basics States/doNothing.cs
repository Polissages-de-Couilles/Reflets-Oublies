using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/base/doNothing")]
public class doNothing : StateBase
{
    public override void Init(GameObject parent, GameObject player)
    {
    }

    public override bool isStateValid()
    {
        return true;
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
