using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosiveMirror : ProjectileBase
{
    [SerializeField] Vector3 centerPos;
    [SerializeField] float spawnRadius;
    [SerializeField] float delayBeforeExplosion;
    bool isDestroyed = false;

    protected override void LaunchProjectile()
    {
        Debug.Log("LAUNCH MIRROR");

        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        transform.position = centerPos + new Vector3(randomPoint.x, 0f, randomPoint.y); // for XZ plane
        Quaternion LookAtRotation = Quaternion.LookRotation(GameManager.Instance.Player.transform.position - transform.position, transform.up);
        LookAtRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = LookAtRotation;

        gameObject.layer = 9;
        attackCollider.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;
        StartCoroutine(DelayExplosion());
    }

    IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(delayBeforeExplosion);
        GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = false;
        isDestroyed = true;
        yield return new WaitForSeconds(4.5f);
        Destroy(gameObject);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
    }
}
