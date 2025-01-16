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
        public Transform grabAnchor;
        [Tooltip("Mettez ici le nombre de secondes avant la fin de l'attaque. Par exemple si l'attaque dure 5 secondes, mettez 1 pour relacher à la 4ème seconde")]
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

