using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public override void Init(List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks)
    {
        this.attacks = projectileAttacks;
        this.doAllAttacks = doAllAttacks;
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
            float animationDuration = animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == animationNames[attack.animationID]).length;
            manager.StartCoroutine(PlayAnimationSpeed(animationDuration, attack));

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
