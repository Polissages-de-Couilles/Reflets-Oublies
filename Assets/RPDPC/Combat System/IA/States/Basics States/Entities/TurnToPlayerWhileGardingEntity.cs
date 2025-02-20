using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnToPlayerWhileGardingEntity : TurnToPlayerEntity
{
    string guardAnim;
    string guardHitAnim;
    GuardManager guardManager;
    Coroutine guardCoroutine;

    public override void Init(float turnDuration, string guardAnim, string guardHitAnim)
    {
        this.turnDuration = turnDuration;
        this.guardAnim = guardAnim;
        this.guardHitAnim = guardHitAnim;
    }

    public override void OnEndState()
    {
        base.OnEndState();
        guardManager.isGuarding = false;
        animator.CrossFadeInFixedTime("GuardEmpty", 0.5f, 1);
        guardManager.asGuarded -= hasGuarded;
        if (guardCoroutine != null)
        {
            manager.StopCoroutine(guardCoroutine); 
            manager.shouldSearchStates = true;
        }
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        guardManager = parent.GetComponent<GuardManager>();
        guardManager.isGuarding = true;
        animator.CrossFadeInFixedTime(guardAnim, 0.5f, 1);
        guardManager.asGuarded += hasGuarded;
    }

    void hasGuarded()
    {
        if (guardCoroutine != null)
        {
            manager.StopCoroutine(guardCoroutine);
            manager.shouldSearchStates = true;
        }
        //guardCoroutine = manager.StartCoroutine(hasGuardedEnum(animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == guardHitAnim).length));
        guardCoroutine = manager.StartCoroutine(hasGuardedEnum(0.56f));
    }

    IEnumerator hasGuardedEnum(float GuardHitAnimLen)
    {
        animator.Play(guardHitAnim);
        manager.shouldSearchStates = false;
        yield return new WaitForSeconds(GuardHitAnimLen);
        manager.shouldSearchStates = true;
        animator.CrossFadeInFixedTime("GuardEmpty", 0.5f, 1);
    }
}
