using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public Action<IDamageable, GameObject> OnDamageableEnterTrigger;
    public Action<GameObject> OnEnterTrigger;

    void OnTriggerEnter(Collider collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if(collider != transform.parent.gameObject || !collider.transform.IsChildOf(transform.parent))
        {
            if (damageable != null)
            {
                OnDamageableEnterTrigger?.Invoke(damageable, gameObject);
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
