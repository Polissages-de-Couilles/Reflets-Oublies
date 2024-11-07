using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC;
using DG.Tweening;

[CreateAssetMenu(menuName = "Game/IA/States/Base/HomingAttack")]
public class HomingAttack : StateBase
{
    [SerializeField] List<AttackDetails> attacks;
    [Tooltip("True if we want the bot to continue to attack, even if the conditions are not met anymore")]
    [SerializeField] bool doAllAttacks;
    int currentIndex = 0;

    GameObject parent;
    StateMachineManager manager;
    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void Init(StateMachineManager manager, GameObject parent, GameObject player)
    {
        this.parent = parent;
        this.manager = manager;
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
        foreach (AttackDetails attack in attacks)
        {
            foreach (AttackColliderDetails collider in attack.colliders)
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

    IEnumerator SpawnCollision(AttackColliderDetails detail)
    {
        yield return new WaitForSeconds(detail.delayBeforeColliderSpawn);

        GameObject attackCollider = Instantiate(new GameObject("BotAttackCollider"), parent.transform);

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
        attackCollider.AddComponent<AttackCollider>().OnDamageableEnterTrigger += DealDamage;

        attackCollider.transform.localPosition = detail.ColliderRelativePosition;
        attackCollider.transform.localRotation = detail.ColliderRelativeRotation;

        yield return attackCollider.transform.DOMoveInTargetLocalSpace(GameManager.Instance.Player.transform, Vector3.zero, DURATION).SetEase(animCurv).WaitForCompletion();
    }

    void DealDamage(IDamageable damageable)
    {
        damageable.takeDamage(attacks[currentIndex].damage);
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
    }

    [Serializable]
    enum ColliderShape
    {
        Box,
        Sphere,
        Capsule
    }
}

