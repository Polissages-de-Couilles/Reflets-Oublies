using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayAnimation : StagingEvent
{
    [SerializeField] Animator animator;
    [SerializeField] string stateName;
    [SerializeField] bool waitAnim = false;

    public override void PlayEvent()
    {
        if (animator == null) 
        {
            DebugError("Invalid animator");
            OnEventFinished?.Invoke();
            return;
        }
        animator.Play(stateName);
        if(waitAnim)
        {
            StartCoroutine(WaitAnim());
        }
        else
        {
            OnEventFinished?.Invoke();
        }
    }

    IEnumerator WaitAnim()
    {
        var time = animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == stateName).length;
        yield return new WaitForSeconds(time);
        OnEventFinished?.Invoke();
    }
}

//animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == guardHitAnim).length