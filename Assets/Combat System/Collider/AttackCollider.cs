using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public Action<IDamageable> OnDamageableEnterTrigger;

    void OnTriggerEnter(Collider collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null && collider != transform.parent.gameObject)
        {
            Debug.Log("Collision");
            OnDamageableEnterTrigger?.Invoke(damageable);
        }
    }

    public void SetCollisionState(bool state)
    {
        GetComponent<Collider>().enabled = state;
    }
}
