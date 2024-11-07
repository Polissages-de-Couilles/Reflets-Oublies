using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public Action<IDamageable> OnDamageableEnterTrigger;
    public Action<GameObject> OnEnterTrigger;

    void OnTriggerEnter(Collider collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if(collider != transform.parent.gameObject || !collider.transform.IsChildOf(transform.parent))
        {
            if (damageable != null)
            {
                Debug.Log("Collision");
                OnDamageableEnterTrigger?.Invoke(damageable);
            }
            OnEnterTrigger?.Invoke(gameObject);
        }
    }

    public void SetCollisionState(bool state)
    {
        GetComponent<Collider>().enabled = state;
    }
}
