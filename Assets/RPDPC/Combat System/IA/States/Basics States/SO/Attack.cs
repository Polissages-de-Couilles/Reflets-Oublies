using ExternPropertyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Attack")]
public class SOAttack : StateBase
{
    [SerializeField] List<AttackDetails> attacks;
    [Tooltip("True if we want the bot to continue to attack, even if the conditions are not met anymore")]
    [SerializeField] bool doAllAttacks;

    public override StateEntityBase PrepareEntityInstance()
    {
        AttackEntity ae = new AttackEntity();
        ae.Init(attacks, doAllAttacks);
        return ae;
    }

    [Serializable]
    public struct AttackDetails
    {
        public List<AttackColliderDetails> colliders;
        public float attackDuration;
        public float damage;
        public bool ShouldOnlyTookDamageFromOneCollision;
        public int animationID;
    }

    [Serializable]
    public struct AttackColliderDetails
    {
        public float delayBeforeColliderSpawn;
        public ColliderShape colliderShape;
        [Tooltip("Also depends of bot rotation. If you put 1 0 0 it will spawn at the x 1 * the bot current rotation. Which is logic")]
        public Vector3 ColliderRelativePosition; // Also depends of Bots rotation
        public Quaternion ColliderRelativeRotation;
        public Vector3 BoxColliderDimension;
        public float CapsuleColliderHeight;
        public float SphereAndCapsuleColliderRadius;
        public float ColliderDuration;
        public bool DoesStun;
        public float StunDuration;
        public bool DoesKnockback;
        public float KnockForce;
        public KnockbackMode KnockbackMode;
    }

    [Serializable]
    public enum ColliderShape
    {
        Box,
        Sphere,
        Capsule
    }
}

