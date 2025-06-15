using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class GW : ProjectileBase
{
    [SerializeField] float waitBeforeLaunch;
    [SerializeField] float duration;
    [SerializeField] Vector3 relativeDestination;
    [SerializeField] float ZRota;
    [SerializeField] GameObject Effect;
    Vector3 effectPos;

    protected override void LaunchProjectile()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<Collider>().enabled = false;
        if (Effect != null) effectPos = Effect.transform.position;

        AttackCollider ac = GetComponentInChildren<Collider>().gameObject.AddComponent<AttackCollider>();
        ac.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        ac.OnDamageableEnterTrigger += TriggerEnter;
        StartCoroutine(launch());
    }

    private void Update()
    {
        if (Effect != null) Effect.transform.position = effectPos;
        transform.Rotate(new Vector3(0, 0, ZRota * Time.deltaTime));
    }

    IEnumerator launch() 
    { 
        if (Effect != null)
        {
            List<ParticleSystemRenderer> part = GetComponentsInChildren<ParticleSystemRenderer>().ToList();
            float timer = 0;
            while (timer <= waitBeforeLaunch)
            {
                foreach (ParticleSystemRenderer part2 in part)
                {
                    part2.material.SetFloat("_RevealAngle", 360 - (360 * (timer / waitBeforeLaunch)));
                }

                timer += Time.deltaTime;
                yield return null;
            }
            foreach (ParticleSystemRenderer part2 in part)
            {
                Destroy(part2.gameObject);
            }
        }
        else
        {
            yield return new WaitForSeconds(waitBeforeLaunch);
        }
        GetComponentInChildren<MeshRenderer>().enabled = true;
        GetComponentInChildren<Collider>().enabled = true;
        yield return transform.DOMove(transform.position + relativeDestination, duration).SetEase(Ease.Linear).WaitForCompletion();
        Destroy(gameObject);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
    }
}
