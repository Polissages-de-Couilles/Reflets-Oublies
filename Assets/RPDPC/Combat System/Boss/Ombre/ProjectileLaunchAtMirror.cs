using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaunchAtMirror : ProjectileBase
{
    [SerializeField] int mirrorToAim;
    [SerializeField] int nbMaxOfBounces;
    [SerializeField] float speed;
    int nbBounces = 0;
    Vector3 direction;
    MirrorsManager mm;

    protected override void LaunchProjectile()
    {
        mm = FindFirstObjectByType<MirrorsManager>();
        direction = (mm.GetMirror(mirrorToAim).transform.position - transform.position).normalized;
        direction.y = 0;

        gameObject.layer = 9;
        attackCollider.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;
        StartCoroutine(MoveProjectile());
    }

    IEnumerator MoveProjectile()
    {
        while (nbBounces <= nbMaxOfBounces)
        {
            transform.position += direction * Time.deltaTime * speed;
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
        if (!manager.damageDetail.doNotDestroyAtTriggerEnter)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Mirror>() != null)
        {
            nbBounces++;
            direction = Vector3.Reflect(direction, other.transform.forward);
            direction.y = 0;
        }
    }
}
