using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Transform Rig => rig;
    [SerializeField] Transform rig;
    public Animator Animator => animator;
    [SerializeField] Animator animator;

    int currentAttackState = 0;

    public void SetSpeed(float speed, Vector3 dir)
    {
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
        animator.SetFloat("Speed", speed);
    }

    public void Roll()
    {
        animator.SetTrigger("Roll");
    }

    public void SetAttackState(int state)
    {
        //Debug.Log($"AttackState : {state}");
        currentAttackState = state;
        animator.SetTrigger($"Attack{currentAttackState} Trigger");
    }

    public void ResetAttack()
    {
        currentAttackState = 0;
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }
}
