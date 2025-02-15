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
    bool isEnemy;
    public List<GameObject> CharacterAlreadyAttacked = new List<GameObject>();

    public void Init(bool DoesStun, float StunDuration, bool DoesKnockback, float KnockForce, KnockbackMode KnockbackMode, bool isEnemy)
    {
        this.DoesStun = DoesStun;
        this.StunDuration = StunDuration;
        this.DoesKnockback = DoesKnockback;
        this.KnockForce = KnockForce;
        this.KnockbackMode = KnockbackMode;
        this.isEnemy = isEnemy;
    }

    void OnTriggerEnter(Collider collider)
    {
        bool changeHasAlreadyTakeDamageValue = false;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        StunAndKnockbackManagerBase SKManager = collider.GetComponent<StunAndKnockbackManagerBase>();
        if(collider != transform.parent.gameObject && !collider.transform.IsChildOf(transform.parent) && (isEnemy ^ (collider.GetComponent<StateMachineManager>() != null)))
        {
            if (damageable != null && !CharacterAlreadyAttacked.Contains(collider.gameObject))
            {
                changeHasAlreadyTakeDamageValue = true;
                OnDamageableEnterTrigger?.Invoke(damageable, gameObject);
            }
            if (SKManager != null && !CharacterAlreadyAttacked.Contains(collider.gameObject))
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
        if(changeHasAlreadyTakeDamageValue)
        {
            CharacterAlreadyAttacked.Add(collider.gameObject);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 1);
    //}

    public void SetCollisionState(bool state)
    {
        GetComponent<Collider>().enabled = state;
    }
}
