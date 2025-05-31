using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class BigBoy : ProjectileBase
{
    [SerializeField] Vector3 randomCenter;
    [SerializeField] float randomRadius;
    [SerializeField] float durationOfFall;
    [SerializeField] float size;
    [SerializeField] Vector3 desiredRotation;

    protected override void LaunchProjectile()
    {
        attackCollider.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;

        Vector3 randomPos = randomCenter + Random.insideUnitSphere * randomRadius;
        transform.position = randomPos;
        transform.rotation = Quaternion.Euler(desiredRotation.x, desiredRotation.y, desiredRotation.z);

        transform.position += -transform.up * 100f;

        GetComponentInChildren<MeshFilter>().gameObject.transform.localScale = new Vector3(size, size, size);

        transform.DOMove(randomPos + transform.up * size / 1.5f, durationOfFall);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
    }
}
