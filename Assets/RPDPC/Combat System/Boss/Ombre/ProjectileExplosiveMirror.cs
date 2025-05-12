using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosiveMirror : ProjectileBase
{
    [SerializeField] Vector3 centerPos;
    [SerializeField] float spawnRadius;
    [SerializeField] float delayBeforeExplosion;

    protected override void LaunchProjectile()
    {
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        transform.position = centerPos + new Vector3(randomPoint.x, 0f, randomPoint.y); // for XZ plane
        transform.rotation = Quaternion.LookRotation(GameManager.Instance.Player.transform.position - transform.position, transform.up);

        gameObject.layer = 9;
        attackCollider.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;
        StartCoroutine(DelayExplosion());
    }

    IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(delayBeforeExplosion);
        GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
    }
}
