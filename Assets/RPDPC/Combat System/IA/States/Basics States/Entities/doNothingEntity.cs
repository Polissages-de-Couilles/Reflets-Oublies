using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doNothingEntity : StateEntityBase
{
    public override void Init()
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
        if(animationNames.Count != 0)
        animator.Play(animationNames[0]);
    }

    public override void OnUpdate()
    {
    }
}
