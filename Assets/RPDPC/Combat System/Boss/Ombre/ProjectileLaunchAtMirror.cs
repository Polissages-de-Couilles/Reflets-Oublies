using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileLaunchAtMirror : ProjectileBase
{
    [SerializeField] int mirrorToAim;
    [SerializeField] int nbMaxOfBounces;
    [SerializeField] float speed;
    
    int nbBounces = 0;
    Vector3 direction;
    MirrorsManager mm;

    float timeWithoutReflect = 0f;

    private void Update()
    {
        timeWithoutReflect += Time.deltaTime;
    }

    protected override void LaunchProjectile()
    {
        mm = FindFirstObjectByType<MirrorsManager>();
        direction = (mm.GetMirror(mirrorToAim).transform.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

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
            if (Vector3.Distance(new Vector3(-230, 63, 584), transform.position) > 30)
            {
                Destroy(gameObject);
            }
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
        if (timeWithoutReflect > 0.2f)
        {
            if (other.gameObject.GetComponent<Mirror>() != null && !other.gameObject.GetComponent<Mirror>().isBroken)
            {
                nbBounces++;
                direction = Vector3.Reflect(direction, other.transform.forward);
                transform.rotation = Quaternion.LookRotation(direction);
                direction.y = 0;
                timeWithoutReflect = 0f;
            }
            else if (other.gameObject.GetComponentInParent<ProjectileExplosiveMirror>() != null && !other.gameObject.GetComponentInParent<ProjectileExplosiveMirror>().IsDestroyed())
            {
                direction = Vector3.Reflect(direction, other.transform.forward);
                direction.y = 0;
                timeWithoutReflect = 0f;
            }
        }
    }
}
