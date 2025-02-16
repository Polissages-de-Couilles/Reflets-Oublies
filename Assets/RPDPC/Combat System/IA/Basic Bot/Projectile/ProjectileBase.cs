using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileManager))]
public abstract class ProjectileBase : MonoBehaviour
{   
    protected ProjectileManager manager;
    protected AttackCollider attackCollider;

    void Start()
    {
        manager = GetComponent<ProjectileManager>();
        attackCollider = gameObject.AddComponent<AttackCollider>();
        LaunchProjectile();
    }

    protected abstract void LaunchProjectile();
}
