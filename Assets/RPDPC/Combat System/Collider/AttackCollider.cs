using System;
using System.Collections;
using System.Collections.Generic;
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
    GameObject Attacker;
    bool isEnemy;
    public List<GameObject> CharacterAlreadyAttacked = new List<GameObject>();
    public GameObject vfx = null;
    public AudioClip sfx = null;

    public void Init(bool DoesStun, float StunDuration, bool DoesKnockback, float KnockForce, KnockbackMode KnockbackMode, bool isEnemy, GameObject Attacker)
    {
        this.DoesStun = DoesStun;
        this.StunDuration = StunDuration;
        this.DoesKnockback = DoesKnockback;
        this.KnockForce = KnockForce;
        this.KnockbackMode = KnockbackMode;
        this.isEnemy = isEnemy;
        this.Attacker = Attacker;
    }

    void OnTriggerEnter(Collider collider)
    {
        bool changeHasAlreadyTakeDamageValue = false;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        StunAndKnockbackManagerBase SKManager = collider.GetComponent<StunAndKnockbackManagerBase>();
        if(collider != Attacker && !collider.transform.IsChildOf(Attacker.transform) && (isEnemy ^ (collider.GetComponent<StateMachineManager>() != null)))
        {
            if (damageable != null && !CharacterAlreadyAttacked.Contains(collider.gameObject))
            {
                changeHasAlreadyTakeDamageValue = true;

                if(vfx != null)
                {
                    Instantiate(vfx, collider.ClosestPointOnBounds(Attacker.transform.position), Quaternion.identity);
                }

                if(sfx != null && this.TryGetComponent(out AudioSource source))
                {
                    source.pitch = UnityEngine.Random.Range(0.75f, 1.25f);
                    source.PlayOneShot(sfx);
                }
                
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
                    SKManager.ApplyKnockback(KnockForce, KnockbackMode, Attacker, collider.gameObject, transform.position);
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
        CharacterAlreadyAttacked.Clear();
        GetComponent<Collider>().enabled = state;
    }
}
