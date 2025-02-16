using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    [SerializeField] Collider attackCollider;
    public int colliderID;

    private void Start()
    {
        attackCollider.enabled = false;
    }

    public void ActivateCollider()
    {
        attackCollider.enabled = true;
    }

    public void DesactivateCollider()
    {
        attackCollider.enabled = false;
    }

    public AttackCollider CreateAttackCollider (bool DoesStun, float StunDuration, bool DoesKnockback, float KnockForce, KnockbackMode KnockbackMode, bool isEnemy)
    {
        AttackCollider ac = GetComponent<AttackCollider>();

        if (ac == null)
        {
            ac = gameObject.AddComponent<AttackCollider>();
        }

        ac.Init(DoesStun, StunDuration, DoesKnockback, KnockForce, KnockbackMode, true, gameObject);

        return ac;
    }
}
