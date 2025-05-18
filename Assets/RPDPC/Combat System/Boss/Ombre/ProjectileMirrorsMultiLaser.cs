using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMirrorsMultiLaser : ProjectileBase
{
    [SerializeField] List<int> mirrors = new List<int>();
    [SerializeField] float duration;
    [SerializeField] float durationBeforeSpawn;
    [SerializeField] float size;
    MirrorsManager mm;

    protected override void LaunchProjectile()
    {
        mm = FindFirstObjectByType<MirrorsManager>();

        gameObject.layer = 9;

        foreach (ProjectileLaunchAtMirror pr in FindObjectsOfType<ProjectileLaunchAtMirror>())
        {
            Destroy(pr.gameObject);
        }
        foreach (ProjectileExplosiveMirror pm in FindObjectsOfType<ProjectileExplosiveMirror>())
        {
            Destroy(pm.gameObject);
        }

        for (int i = 1; i < mirrors.Count; i++)
        {
            Mirror firstMirror = mm.GetMirror(mirrors[i - 1]);
            Mirror lastMirror = mm.GetMirror(mirrors[i]);

            if (!(firstMirror == null || lastMirror == null || firstMirror.isBroken || lastMirror.isBroken))
            {
                GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                capsule.GetComponent<CapsuleCollider>().enabled = false;
                capsule.GetComponent<CapsuleCollider>().isTrigger = true;
                AttackCollider ac = capsule.AddComponent<AttackCollider>();
                ac.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
                ac.OnDamageableEnterTrigger += TriggerEnter;
                MirrorLaser ml = capsule.AddComponent<MirrorLaser>();
                ml.InitLaser(mirrors[i - 1], mirrors[i], duration, durationBeforeSpawn, size, mm);
            }
        }

        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(durationBeforeSpawn + duration);
        Destroy(gameObject);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
    }
}
