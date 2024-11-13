using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackEntity : StateEntityBase
{
    List<SOAttack.AttackDetails> attacks;
    bool doAllAttacks;

    Dictionary<GameObject, SOAttack.AttackDetails> currentAttacks = new Dictionary<GameObject, SOAttack.AttackDetails>();
    Dictionary<SOAttack.AttackDetails, bool> attackAlreadyDealtDamage = new Dictionary<SOAttack.AttackDetails, bool>();
    bool finishedSpawnAllAttacks = false;
    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves, GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns)
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
        manager.shouldSearchStates = false;
        finishedSpawnAllAttacks = false;
        foreach (SOAttack.AttackDetails attack in attacks)
        {
            if(attackAlreadyDealtDamage.ContainsKey(attack))
            {
                attackAlreadyDealtDamage.Remove(attack);
            }
            attackAlreadyDealtDamage.Add(attack, false);
        }
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
                manager.StartCoroutine(SpawnCollision(collider, attack));
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
        }
        finishedSpawnAllAttacks = true;
    }

    IEnumerator SpawnCollision(SOAttack.AttackColliderDetails detail, SOAttack.AttackDetails ad)
    {
        yield return new WaitForSeconds(detail.delayBeforeColliderSpawn);

        GameObject attackCollider = new GameObject("BotAttackCollider");
        attackCollider.transform.parent = parent.transform;
        currentAttacks.Add(attackCollider, ad);

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

        if(currentAttacks.Count == 1 && finishedSpawnAllAttacks)
        {
            ExitState();
        }

        currentAttacks.Remove(attackCollider);
        if (attackCollider != null)
        {
            Debug.Log(attackCollider);
            MonoBehaviour.Destroy(attackCollider);
        }
    }

    void DealDamage(IDamageable damageable, GameObject collider) {
        if (!attackAlreadyDealtDamage[currentAttacks[collider]])
        {
            attackAlreadyDealtDamage[currentAttacks[collider]] = true;
            damageable.takeDamage(currentAttacks[collider].damage);
        }
    }
}

