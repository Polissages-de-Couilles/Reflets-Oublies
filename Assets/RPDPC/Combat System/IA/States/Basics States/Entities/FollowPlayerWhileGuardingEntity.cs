using System.Collections;
using System.Linq;
using UnityEngine;

public class FollowPlayerWhileGuardingEntity : FollowPlayerEntity
{
    string guardAnim;
    string guardHitAnim;
    GuardManager guardManager;
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
        animator.Play("GuardEmpty");
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
        manager.StartCoroutine(hasGuardedEnum(animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == guardHitAnim).length));
    }

    IEnumerator hasGuardedEnum(float GuardHitAnimLen)
    {
        animator.Play(guardHitAnim);
        isGuardingHit = true;
        yield return new WaitForSeconds(GuardHitAnimLen);
        isGuardingHit = false;
        playAnim();
    }

    override protected void playAnim()
    {
        animator.CrossFadeInFixedTime(animationNames[0], 0.5f);
        animator.CrossFadeInFixedTime(guardAnim, 0.5f, 1);
    }
}
