using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingEntity : StateEntityBase
{
    public override void ExitState()
    {
    }

    public override void Init()
    {
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
        manager.shouldSearchStates = false;
    }

    public override void OnUpdate()
    {
        if (!isStateValid())
        {
            manager.shouldSearchStates = true;
        }
    }
}
