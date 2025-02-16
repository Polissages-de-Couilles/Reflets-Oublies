using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ProjectileLaunchAtPlayer : ProjectileBase
{
    [SerializeField] float lifespan;
    [SerializeField] float speed;

    protected override void LaunchProjectile()
    {
        gameObject.layer = 9;
        attackCollider.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;
        StartCoroutine(MoveProjectile());
    }

    IEnumerator MoveProjectile()
    {
        float timer = 0f;
        Vector3 direction = (manager.target.transform.position - transform.position).normalized;
        while (timer < lifespan)
        {
            transform.position += direction * Time.deltaTime * speed;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
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
}
