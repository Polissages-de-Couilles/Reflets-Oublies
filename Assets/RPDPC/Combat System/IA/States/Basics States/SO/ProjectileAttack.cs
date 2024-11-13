using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC;
using DG.Tweening;

[CreateAssetMenu(menuName = "Game/IA/States/Base/ProjectileAttack")]
public class SOProjectileAttack : StateBase
{
    [SerializeField] List<SOProjectileAttack.ProjectileAttackDetails> attacks;
    [Tooltip("True if we want the bot to continue to attack, even if the conditions are not met anymore")]
    [SerializeField] bool doAllAttacks;

    public override StateEntityBase PrepareEntityInstance()
    {
        ProjectileAttackEntity pa = new ProjectileAttackEntity();
        pa.Init(false, null, attacks, doAllAttacks, new Vector3(), 0, false, false, new Vector2(), null, 0, 0, 0, Vector2.zero);
        return pa;
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

