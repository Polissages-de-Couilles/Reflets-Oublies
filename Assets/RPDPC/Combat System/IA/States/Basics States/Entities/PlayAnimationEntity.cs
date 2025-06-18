using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class PlayAnimationEntity : StateEntityBase
{
    string animName;
    AnimationClip clip;
    float setDuration;
    int layer;
    float speed;
    string speedPararemerName;
    float crosstimeduration;

    public override void Init(string animName, AnimationClip clip, float setDuration, int layer, float speed, string speedPararemerName, float crosstimeduration)
    { 
        this.animName = animName;
        this.clip = clip;
        this.setDuration = setDuration;
        this.layer = layer;
        this.speed = speed;
        this.speedPararemerName = speedPararemerName;
        this.crosstimeduration = crosstimeduration;
    }

    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
        manager.shouldSearchStates = true;
    }

    public override void OnEnterState()
    {
        animator.SetFloat(speedPararemerName, 1);
        manager.shouldSearchStates = false;
        manager.StartCoroutine(PlayAnimation());
    }

    public override void OnUpdate()
    {
    }

    public IEnumerator PlayAnimation()
    {
        if (setDuration == -1)
        {
            animator.SetFloat(speedPararemerName, speed);
            animator.CrossFade(animName, crosstimeduration, layer);
            yield return new WaitForSeconds(clip.length / speed);
        }
        else
        {
            animator.SetFloat(speedPararemerName, /*animDuration/setDuration*/ speed);
            animator.CrossFade(animName, crosstimeduration, layer);
            yield return new WaitForSeconds(setDuration);
        }
        ExitState();
    }
}
