using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LaserEllessard : ProjectileBase
{
    [SerializeField] float lifespan;

    protected override void LaunchProjectile()
    {
        gameObject.layer = 9;
        gameObject.transform.Rotate(gameObject.transform.rotation.eulerAngles.x,manager.launcher.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);
        attackCollider.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;
        StartCoroutine(MoveProjectile());
    }

    IEnumerator MoveProjectile()
    {
        float timer = 0f;
        while (timer < lifespan)
        {
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
