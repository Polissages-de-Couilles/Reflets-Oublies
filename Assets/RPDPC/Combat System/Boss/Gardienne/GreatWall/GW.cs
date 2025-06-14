using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GW : ProjectileBase
{
    [SerializeField] float duration;
    [SerializeField] Vector3 relativeDestination;
    [SerializeField] float ZRota;

    protected override void LaunchProjectile()
    {
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;
        StartCoroutine(launch());
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, ZRota * Time.deltaTime));
    }

    IEnumerator launch() 
    { 
        yield return transform.DOMove(transform.position + relativeDestination, duration).SetEase(Ease.Linear).WaitForCompletion();
        Destroy(gameObject);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
    }
}
