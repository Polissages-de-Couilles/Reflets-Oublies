using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.ParticleSystem;

public class ProjectileAttackEntity : StateEntityBase
{
    List<SOProjectileAttack.ProjectileAttackDetails> attacks;
    bool doAllAttacks;
    Vector2 timeWithoutAttackAfter;

    Dictionary<GameObject, SOProjectileAttack.ProjectileAttackDetails> currentAttacks = new Dictionary<GameObject, SOProjectileAttack.ProjectileAttackDetails>();
    bool finishedSpawnAllAttacks = false;
    
    public override void ExitState()
    {
        onActionFinished?.Invoke();
        manager.StopPrioritizeAttack(UnityEngine.Random.Range(timeWithoutAttackAfter.x, timeWithoutAttackAfter.y));
    }

    public override void Init(List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector2 timeWithoutAttackAfter)
    {
        this.attacks = projectileAttacks;
        this.doAllAttacks = doAllAttacks;
        this.timeWithoutAttackAfter = timeWithoutAttackAfter;
    }

    public override void OnEndState()
    {
        manager.shouldSearchStates = true;
        manager.StopCoroutine(SpawnAttack());
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
            if (animator != null)
            {
                float animationDuration = animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == animationNames[attack.animationID]).length;
                manager.StartCoroutine(PlayAnimationSpeed(animationDuration, attack));
            }

            /// VFX
            if (attack.VFX != null)
            {
                ProjectileSpawner ps = parent.GetComponentsInChildren<ProjectileSpawner>().ToList().Find(x => x.spawnerID == attack.spawnerID);
                GameObject vfx = MonoBehaviour.Instantiate(attack.VFX, ps.gameObject.transform.position, Quaternion.identity);
                List<ParticleSystem> particles = new List<ParticleSystem>();
                particles.AddRange(vfx.GetComponents<ParticleSystem>());
                particles.AddRange(vfx.GetComponentsInChildren<ParticleSystem>());

                foreach (ParticleSystem particle in particles)
                {
                    var main = particle.main;
                    main.duration = attack.attackDuration;
                }
                particles[0].Play();
            }
            /// 


            foreach (SOProjectileAttack.ProjectileAttackPrefabDetails collider in attack.prefabProjectiles)
            {
                manager.StartCoroutine(SpawnCollision(collider, attack));
            }
            
            yield return new WaitForSeconds(attack.attackDuration);

        }
        ExitState();
    }

    IEnumerator PlayAnimationSpeed(float animDuration, SOProjectileAttack.ProjectileAttackDetails pad)
    {
        float timer = 0f;
        animator.Play(animationNames[pad.animationID]);
        while (timer < animDuration)
        {
            animator.SetFloat("AttackSpeed", pad.animationSpeed.Evaluate(timer / animDuration));
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime * pad.animationSpeed.Evaluate(timer / animDuration);
        }
        animator.Play(animationNames[0]);
    }

    IEnumerator SpawnCollision(SOProjectileAttack.ProjectileAttackPrefabDetails papd, SOProjectileAttack.ProjectileAttackDetails ad)
    {
        yield return new WaitForSeconds(papd.delayBeforeSpawn);

        ProjectileSpawner ps = parent.GetComponentsInChildren<ProjectileSpawner>().ToList().Find(x => x.spawnerID == ad.spawnerID);

        GameObject projectile = MonoBehaviour.Instantiate(papd.prefab, ps.gameObject.transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileManager>().Init(parent, player, papd.damageDetails);
    }
}
