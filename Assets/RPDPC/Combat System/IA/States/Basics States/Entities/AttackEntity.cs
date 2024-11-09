using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackEntity : StateEntityBase
{
    List<SOAttack.AttackDetails> attacks;
    bool doAllAttacks;
    int currentIndex = 0;
    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves)
    {
        this.attacks = attacks;
        this.doAllAttacks = doAllAttacks;
    }

    public override void OnEndState()
    {
        manager.shouldSearchStates = true;
    }

    public override void OnEnterState()
    {
        currentIndex = 0;
        manager.shouldSearchStates = false;
        manager.StartCoroutine(SpawnAttack());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator SpawnAttack()
    {
        foreach (SOAttack.AttackDetails attack in attacks)
        {
            foreach(SOAttack.AttackColliderDetails collider in attack.colliders)
            {
                manager.StartCoroutine(SpawnCollision(collider));
            }

            yield return new WaitForSeconds(attack.attackDuration);

            if (!doAllAttacks)
            {
                if (!isStateValid())
                {
                    ExitState();
                    break;
                }
            }

            currentIndex++;
        }
    }

    IEnumerator SpawnCollision(SOAttack.AttackColliderDetails detail)
    {
        yield return new WaitForSeconds(detail.delayBeforeColliderSpawn);

        GameObject attackCollider = MonoBehaviour.Instantiate(new GameObject("BotAttackCollider"), parent.transform);
        
        switch (detail.colliderShape)
        {
            case SOAttack.ColliderShape.Box:
                BoxCollider boxCollider = attackCollider.AddComponent<BoxCollider>();
                boxCollider.size = detail.BoxColliderDimension;
                boxCollider.isTrigger = true;
                break;

            case SOAttack.ColliderShape.Sphere:
                SphereCollider sphereCollider = attackCollider.AddComponent<SphereCollider>();
                sphereCollider.radius = detail.SphereAndCapsuleColliderRadius;
                sphereCollider.isTrigger = true;
                break;

            case SOAttack.ColliderShape.Capsule:
                CapsuleCollider capsuleCollider = attackCollider.AddComponent<CapsuleCollider>();
                capsuleCollider.radius = detail.SphereAndCapsuleColliderRadius;
                capsuleCollider.height = detail.CapsuleColliderHeight;
                capsuleCollider.isTrigger = true;
                break;
        }
        attackCollider.AddComponent<AttackCollider>().OnDamageableEnterTrigger += DealDamage;

        attackCollider.transform.localPosition = detail.ColliderRelativePosition;
        attackCollider.transform.localRotation = detail.ColliderRelativeRotation;

        yield return new WaitForSeconds(detail.ColliderDuration);

        MonoBehaviour.Destroy(attackCollider);
    }

    void DealDamage(IDamageable damageable, GameObject collider) {
        damageable.takeDamage(attacks[currentIndex].damage);
    }
}

