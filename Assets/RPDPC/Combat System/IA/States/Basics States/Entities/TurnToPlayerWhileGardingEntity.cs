using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPlayerWhileGardingEntity : TurnToPlayerEntity
{
    string guardAnim;
    string guardHitAnim;
    GuardManager guardManager;

    public override void OnEndState()
    {
        base.OnEndState();
        guardManager.isGuarding = false;
        animator.CrossFadeInFixedTime("GuardEmpty", 0.5f, 1);
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        guardManager = parent.GetComponent<GuardManager>();
        guardManager.isGuarding = true;
        animator.CrossFadeInFixedTime(guardAnim, 0.5f, 1);
    }
}
