using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEntity : StateEntityBase
{
    string guardAnim;
    string guardHitAnim;
    GuardManager guardManager;
    Coroutine guardCoroutine;

    public override void Init(string guardAnim, string guardHitAnim)
    {
        this.guardAnim = guardAnim;
        this.guardHitAnim = guardHitAnim;
    }

    public override void OnEndState()
    {
        guardManager.isGuarding = false;
        guardManager.asGuarded -= hasGuarded;
        animator.CrossFadeInFixedTime("GuardEmpty", 0.5f, 1);
        if (guardCoroutine != null)
        {
            manager.StopCoroutine(guardCoroutine);
            manager.shouldSearchStates = true;
        }
    }

    public override void OnEnterState()
    {
        guardManager = parent.GetComponent<GuardManager>();
        guardManager.isGuarding = true;
        guardManager.asGuarded += hasGuarded;
        animator.CrossFadeInFixedTime(animationNames[0], 0);
        animator.CrossFadeInFixedTime(guardAnim, 0.5f, 1);
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
        animator.Play(animationNames[0], 0);
        animator.CrossFadeInFixedTime(guardAnim, 0.5f, 1);
        yield return new WaitForSeconds(0.5f);
        manager.shouldSearchStates = true;
    }

    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void OnUpdate()
    {
    }
}
