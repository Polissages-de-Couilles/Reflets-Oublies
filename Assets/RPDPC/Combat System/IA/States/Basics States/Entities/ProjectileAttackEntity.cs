using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC;
using DG.Tweening;

public class ProjectileAttackEntity : StateEntityBase
{
    List<SOProjectileAttack.ProjectileAttackDetails> attacks;
    bool doAllAttacks;

    Dictionary<GameObject, SOProjectileAttack.ProjectileAttackDetails> currentAttacks = new Dictionary<GameObject, SOProjectileAttack.ProjectileAttackDetails>();
    bool finishedSpawnAllAttacks = false;
    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves, GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns)
    {
        this.attacks = projectileAttacks;
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
        manager.StartCoroutine(SpawnAttack());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator SpawnAttack()
    {
        foreach (SOProjectileAttack.ProjectileAttackDetails attack in attacks)
        {
            foreach (SOProjectileAttack.ProjectileAttackColliderDetails collider in attack.colliders)
            {
                manager.StartCoroutine(SpawnCollision(collider, attack));
            }

            yield return new WaitForSeconds(attack.attackDuration);

            if (!doAllAttacks)
            {
                if (!isStateValid())
                {
                    //ExitState();
                    manager.shouldSearchStates = true;
                    break;
                }
            }
        }
        //finishedSpawnAllAttacks = true;
        manager.shouldSearchStates = true;
    }

    IEnumerator SpawnCollision(SOProjectileAttack.ProjectileAttackColliderDetails detail, SOProjectileAttack.ProjectileAttackDetails ad)
    {
        yield return new WaitForSeconds(detail.delayBeforeColliderSpawn);

        GameObject attackCollider = new GameObject("BotAttackCollider");
        attackCollider.transform.parent = parent.transform;
        currentAttacks.Add(attackCollider, ad);

        switch (detail.colliderShape)
        {
            case SOProjectileAttack.ProjectileColliderShape.Box:
                BoxCollider boxCollider = attackCollider.AddComponent<BoxCollider>();
                boxCollider.size = detail.BoxColliderDimension;
                boxCollider.isTrigger = true;
                break;

            case SOProjectileAttack.ProjectileColliderShape.Sphere:
                SphereCollider sphereCollider = attackCollider.AddComponent<SphereCollider>();
                sphereCollider.radius = detail.SphereAndCapsuleColliderRadius;
                sphereCollider.isTrigger = true;
                break;

            case SOProjectileAttack.ProjectileColliderShape.Capsule:
                CapsuleCollider capsuleCollider = attackCollider.AddComponent<CapsuleCollider>();
                capsuleCollider.radius = detail.SphereAndCapsuleColliderRadius;
                capsuleCollider.height = detail.CapsuleColliderHeight;
                capsuleCollider.isTrigger = true;
                break;
        }

        //Pour décembre : Visuel PlaceHolder
        GameObject mesh = new GameObject("mesh");
        mesh.transform.parent = attackCollider.transform;
        Material material = new Material(Shader.Find("FlatKit/Stylized Surface"));
        material.color = Color.red;
        mesh.AddComponent<MeshRenderer>().material = material;
        mesh.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Capsule.fbx");
        mesh.transform.localScale = new Vector3(0.1f, 0, 0.3f);

        AttackCollider ac = attackCollider.AddComponent<AttackCollider>();
        ac.Init(detail.DoesStun, detail.StunDuration, detail.DoesKnockback, detail.KnockForce, detail.KnockbackMode, true);
        ac.OnDamageableEnterTrigger += DealDamage;
        ac.OnEnterTrigger += DestroyCollision;

        attackCollider.transform.localPosition = detail.ColliderRelativePosition;
        attackCollider.transform.localRotation = detail.ColliderRelativeRotation;

        Vector3 vDistance = (attackCollider.transform.position - player.transform.position);
        vDistance = new Vector3(-vDistance.x, vDistance.y, -vDistance.z).normalized;

        yield return attackCollider.transform.DOMove(attackCollider.transform.position + new Vector3(vDistance.x * detail.distance, vDistance.y, vDistance.z * detail.distance), detail.distance / detail.speed).SetEase(detail.animCurv).WaitForCompletion();

        //if (currentAttacks.Count <= 1 && finishedSpawnAllAttacks)
        //{
        //    ExitState();
        //}

        currentAttacks.Remove(attackCollider);
        if(attackCollider != null)
        {
            MonoBehaviour.Destroy(attackCollider);
        }
    }

    void DealDamage(IDamageable damageable, GameObject collider)
    {
        damageable.takeDamage(currentAttacks[collider].damage);
    }

    void DestroyCollision(GameObject collision)
    {
        Debug.Log("TOUCHED SOMETHING " + collision);
        MonoBehaviour.Destroy(collision);
    }

    [Serializable]
    public struct ProjectileAttackDetails
    {
        public List<ProjectileAttackColliderDetails> colliders;
        public float attackDuration;
        public float damage;
    }

    [Serializable]
    public struct ProjectileAttackColliderDetails
    {
        public float delayBeforeColliderSpawn;
        public ProjectileColliderShape colliderShape;
        [Tooltip("Also depends of bot rotation. If you put 1 0 0 it will spawn at the x 1 * the bot current rotation. Which is logic")]
        public Vector3 ColliderRelativePosition; // Also depends of Bots rotation
        public Quaternion ColliderRelativeRotation;
        public Vector3 BoxColliderDimension;
        public float CapsuleColliderHeight;
        public float SphereAndCapsuleColliderRadius;
        public float distance;
        public float speed;
        public AnimationCurve animCurv;
    }

    [Serializable]
    public enum ProjectileColliderShape
    {
        Box,
        Sphere,
        Capsule
    }
}

