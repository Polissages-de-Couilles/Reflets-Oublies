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
        public GrabDetails grabDetails;
    }

    [Serializable]
    public struct GrabDetails
    {
        public int grabID;
        [Tooltip("Temps par rapport à durer de l'attaque, par exemple si l'attaque 5 secondes, mettre 4 arrête le grab à la 4ème seconde, peut importe que le joueur ait été grab à la 2ème ou 3ème secondes")]
        public float grabReleaseTime;
        public Vector3 grabReleaseForce;
        public float grabStunDuration;
    }

    [Serializable]
    public struct AttackColliderDetails
    {
        public float delayBeforeColliderSpawn;
        public float colliderID;
        public float ColliderDuration;
        public bool DoesStun;
        public float StunDuration;
        public bool DoesKnockback;
        public float KnockForce;
        public KnockbackMode KnockbackMode;
        public GameObject VFX;
    }

    [Serializable]
    public enum ColliderShape
    {
        Box,
        Sphere,
        Capsule
    }
}

