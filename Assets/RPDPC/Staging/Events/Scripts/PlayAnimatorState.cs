using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : StagingEvent
{
    [SerializeField] Animator animator;
    [SerializeField] string stateName;

    public override void PlayEvent()
    {
        if (animator == null) 
        {
            DebugError("Invalid animator");
            OnEventFinished?.Invoke();
            return;
        }
        animator.Play(stateName);
        OnEventFinished?.Invoke();
    }
}

//animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == guardHitAnim).length