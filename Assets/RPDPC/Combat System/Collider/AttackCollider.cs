using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public Action<IDamageable, GameObject> OnDamageableEnterTrigger;
    public Action<GameObject> OnEnterTrigger;

    public bool DoesStun;
    public float StunDuration;
    public bool DoesKnockback;
    public float KnockForce;
    public KnockbackMode KnockbackMode;

    public void Init(bool DoesStun, float StunDuration, bool DoesKnockback, float KnockForce, KnockbackMode KnockbackMode)
    {
        this.DoesStun = DoesStun;
        this.StunDuration = StunDuration;
        this.DoesKnockback = DoesKnockback;
        this.KnockForce = KnockForce;
        this.KnockbackMode = KnockbackMode;
    }

    void OnTriggerEnter(Collider collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();
        StunAndKnockbackManagerBase SKManager = collider.GetComponent<StunAndKnockbackManagerBase>();
        if(collider != transform.parent.gameObject && !collider.transform.IsChildOf(transform.parent))
        {
            if (damageable != null)
            {
                OnDamageableEnterTrigger?.Invoke(damageable, gameObject);
            }
            if (SKManager != null)
            {
                if (DoesStun)
                {
                    SKManager.ApplyStun(StunDuration);
                }
                if (DoesKnockback)
                {
                    SKManager.ApplyKnockback(KnockForce, KnockbackMode, transform.parent.gameObject, collider.gameObject, collider.transform.position);
                }
            }
            OnEnterTrigger?.Invoke(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1);
    }

    public void SetCollisionState(bool state)
    {
        GetComponent<Collider>().enabled = state;
    }
}
