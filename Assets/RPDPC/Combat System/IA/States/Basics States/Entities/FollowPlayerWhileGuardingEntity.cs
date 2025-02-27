using System.Collections;
using System.Linq;
using UnityEngine;

public class FollowPlayerWhileGuardingEntity : FollowPlayerEntity
{
    string guardAnim;
    string guardHitAnim;
    GuardManager guardManager;
    Coroutine guardCoroutine;
    bool isGuardingHit = false;

    public override void Init(bool isIntelligent, string guardAnim, string guardHitAnim)
    {
        this.isIntelligent = isIntelligent;
        this.guardAnim = guardAnim;
        this.guardHitAnim = guardHitAnim;
    }

    public override void OnEndState()
    {
        base.OnEndState();
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
        base.OnEnterState();
        guardManager = parent.GetComponent<GuardManager>();
        guardManager.isGuarding = true;
        guardManager.asGuarded += hasGuarded;
    }

    public override void OnUpdate()
    {
        if (!isGuardingHit)
        {
            base.OnUpdate();
        }
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
        isGuardingHit = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(GuardHitAnimLen);
        playAnim();
        yield return new WaitForSeconds(Mathf.Max(animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == animationNames[0]).length, animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == guardAnim).length));
        manager.shouldSearchStates = true;
        agent.isStopped = false;
        isGuardingHit = false;
    }

    override protected void playAnim()
    {
        animator.CrossFadeInFixedTime(animationNames[0], 0.5f, 0);
        animator.CrossFadeInFixedTime(guardAnim, 0.5f, 1);
    }
}
