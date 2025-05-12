using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Transform Rig => rig;
    [SerializeField] Transform rig;
    [SerializeField] Animator animator;

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

    public void Attack()
    {

    }
}
