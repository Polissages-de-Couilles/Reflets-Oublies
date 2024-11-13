using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobSpawnerEntity : StateEntityBase
{
    GameObject monsterPrefab;
    int nbToSpawnAtEnterState;
    int mobMaxNb;
    float spawnRange;
    Vector2 rangeTimeBetweenSpawns;

    List<GameObject> spawnedMobs = new List<GameObject>();
    public override void ExitState()
    {
    }

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves, GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns)
    {
        this.monsterPrefab = monsterPrefab;
        this.nbToSpawnAtEnterState = nbToSpawnAtEnterState;
        this.mobMaxNb = mobMaxNb;
        this.spawnRange = spawnRange;
        this.rangeTimeBetweenSpawns = rangeTimeBetweenSpawns;
    }

    public override void OnEndState()
    {
        manager.StopCoroutine(spawnMobsEveryTimeRange());
    }

    public override void OnEnterState()
    {
        if (ValidateAndGetSpawnedMobsCount() < nbToSpawnAtEnterState) 
        { 
            for (int i = 0; i < nbToSpawnAtEnterState; i++)
            {
                if (ValidateAndGetSpawnedMobsCount() < mobMaxNb)
                {
                    SpawnMob();
                }
            }
        }

        manager.StartCoroutine(spawnMobsEveryTimeRange());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator spawnMobsEveryTimeRange() 
    { 
        yield return new WaitForSeconds(Random.Range(rangeTimeBetweenSpawns.x, rangeTimeBetweenSpawns.y));
        yield return new WaitUntil(verifyMobNumber);
        SpawnMob();
        manager.StartCoroutine(spawnMobsEveryTimeRange());
    }

    void SpawnMob()
    {
        Vector2 RandomPoint = Random.insideUnitCircle * spawnRange;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(parent.transform.position + new Vector3(RandomPoint.x, 0, RandomPoint.y), out hit, spawnRange, NavMesh.AllAreas))
        {
            GameObject mob = MonoBehaviour.Instantiate(monsterPrefab);
            mob.transform.position = new Vector3(hit.position.x, hit.position.y + mob.GetComponent<Collider>().bounds.size.y, hit.position.z);
            spawnedMobs.Add(mob);
            mob.name = mob.name + " " + spawnedMobs.IndexOf(mob);
        }
    }

    int ValidateAndGetSpawnedMobsCount()
    {
        foreach (GameObject mob in spawnedMobs)
        {
            if(mob == null)
            {
                spawnedMobs.Remove(mob);
            }
        }
        return spawnedMobs.Count;
    }

    bool verifyMobNumber()
    {
        return ValidateAndGetSpawnedMobsCount() < mobMaxNb;
    }
}
