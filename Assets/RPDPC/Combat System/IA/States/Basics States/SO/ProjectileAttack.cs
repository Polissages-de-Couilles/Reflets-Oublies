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
        public float damage;
    }

    [Serializable]
    public struct ProjectileAttackPrefabDetails
    {
        public GameObject prefab;
        public float delayBeforeSpawn;
    }
}

