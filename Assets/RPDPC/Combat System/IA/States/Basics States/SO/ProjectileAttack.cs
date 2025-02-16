using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC;
using DG.Tweening;
using UnityEditor;

[CreateAssetMenu(menuName = "Game/IA/States/Base/ProjectileAttack")]
public class SOProjectileAttack : StateBase
{
    [SerializeField] List<SOProjectileAttack.ProjectileAttackDetails> attacks;
    [Tooltip("True if we want the bot to continue to attack, even if the conditions are not met anymore")]
    [SerializeField] bool doAllAttacks;

    public override StateEntityBase PrepareEntityInstance()
    {
        ProjectileAttackEntity pa = new ProjectileAttackEntity();
        pa.Init(attacks, doAllAttacks);
        return pa;
    }

    [Serializable]
    public struct ProjectileAttackDetails
    {
        public int spawnerID;
        public List<ProjectileAttackPrefabDetails> prefabProjectiles;
        public float attackDuration;
        public int animationID;
        public AnimationCurve animationSpeed;
    }

    [Serializable]
    public struct ProjectileAttackPrefabDetails
    {
        public GameObject prefab;
        public float delayBeforeSpawn;
        public ProjectileAttackDamageDetails damageDetails;
    }
}

[Serializable]
public struct ProjectileAttackDamageDetails
{
    public float damage;
    public bool doesStun;
    public float stunDuration;
    public bool doesKnockback;
    public float knockbackForce;
    public bool doNotDestroyAtTriggerEnter;
}

