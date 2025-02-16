using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    [SerializeField] Collider attackCollider;
    public int colliderID;
    AttackCollider ac;

    private void Start()
    {
        attackCollider.enabled = false;
    }

    public void ActivateCollider()
    {
        ac.SetCollisionState(true);
    }

    public void DesactivateCollider()
    {
        ac.SetCollisionState(false);
    }

    public AttackCollider CreateAttackCollider (bool DoesStun, float StunDuration, bool DoesKnockback, float KnockForce, KnockbackMode KnockbackMode, bool isEnemy, GameObject parent)
    {
        ac = GetComponent<AttackCollider>();

        if (ac == null)
        {
            ac = gameObject.AddComponent<AttackCollider>();
        }

        ac.Init(DoesStun, StunDuration, DoesKnockback, KnockForce, KnockbackMode, true, parent);

        return ac;
    }
}
