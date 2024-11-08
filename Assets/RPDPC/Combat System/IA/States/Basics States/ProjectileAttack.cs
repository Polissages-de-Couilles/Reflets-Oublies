using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC;
using DG.Tweening;

[CreateAssetMenu(menuName = "Game/IA/States/Base/ProjectileAttack")]
public class ProjectileAttack : StateBase
{
    [SerializeField] List<AttackDetails> attacks;
    [Tooltip("True if we want the bot to continue to attack, even if the conditions are not met anymore")]
    [SerializeField] bool doAllAttacks;

    Dictionary<GameObject, AttackDetails> currentAttacks = new Dictionary<GameObject, AttackDetails>();
    GameObject parent;
    GameObject player;
    StateMachineManager manager;
    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void Init(StateMachineManager manager, GameObject parent, GameObject player)
    {
        this.parent = parent;
        this.manager = manager;
        this.player = player;
    }

    public override void OnEndState()
    {
        manager.shouldSearchStates = true;
    }

    public override void OnEnterState()
    {
        manager.shouldSearchStates = false;
        manager.StartCoroutine(SpawnAttack());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator SpawnAttack()
    {
        foreach (AttackDetails attack in attacks)
        {
            foreach (AttackColliderDetails collider in attack.colliders)
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
    }

    IEnumerator SpawnCollision(AttackColliderDetails detail, AttackDetails ad)
    {
        yield return new WaitForSeconds(detail.delayBeforeColliderSpawn);

        GameObject attackCollider = Instantiate(new GameObject("BotAttackCollider"), parent.transform);
        currentAttacks.Add(attackCollider, ad);

        switch (detail.colliderShape)
        {
            case ColliderShape.Box:
                BoxCollider boxCollider = attackCollider.AddComponent<BoxCollider>();
                boxCollider.size = detail.BoxColliderDimension;
                boxCollider.isTrigger = true;
                break;

            case ColliderShape.Sphere:
                SphereCollider sphereCollider = attackCollider.AddComponent<SphereCollider>();
                sphereCollider.radius = detail.SphereAndCapsuleColliderRadius;
                sphereCollider.isTrigger = true;
                break;

            case ColliderShape.Capsule:
                CapsuleCollider capsuleCollider = attackCollider.AddComponent<CapsuleCollider>();
                capsuleCollider.radius = detail.SphereAndCapsuleColliderRadius;
                capsuleCollider.height = detail.CapsuleColliderHeight;
                capsuleCollider.isTrigger = true;
                break;
        }
        AttackCollider ac = attackCollider.AddComponent<AttackCollider>();
        ac.OnDamageableEnterTrigger += DealDamage;
        ac.OnEnterTrigger += DestroyCollision;

        attackCollider.transform.localPosition = detail.ColliderRelativePosition;
        attackCollider.transform.localRotation = detail.ColliderRelativeRotation;

        Vector3 vDistance = (attackCollider.transform.position - player.transform.position);
        vDistance = new Vector3(-vDistance.x, vDistance.y, -vDistance.z).normalized;
        Debug.Log(vDistance);

        yield return attackCollider.transform.DOMove(new Vector3(vDistance.x * detail.distance, vDistance.y, vDistance.z * detail.distance), detail.distance / detail.speed).SetEase(detail.animCurv).WaitForCompletion();
        //yield return attackCollider.transform.DOMove(-(attackCollider.transform.position - player.transform.position).normalized * detail.distance, detail.distance / detail.speed).WaitForCompletion();

        currentAttacks.Remove(attackCollider);
        Destroy(attackCollider);
    }

    void DealDamage(IDamageable damageable, GameObject collider)
    {
        damageable.takeDamage(currentAttacks[collider].damage);
    }

    void DestroyCollision(GameObject collision)
    {
        Debug.Log("TOUCHED SOMETHING " + collision);
        Destroy(collision);
    }

    [Serializable]
    struct AttackDetails
    {
        public List<AttackColliderDetails> colliders;
        public float attackDuration;
        public float damage;
    }

    [Serializable]
    struct AttackColliderDetails
    {
        public float delayBeforeColliderSpawn;
        public ColliderShape colliderShape;
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
    enum ColliderShape
    {
        Box,
        Sphere,
        Capsule
    }
}

