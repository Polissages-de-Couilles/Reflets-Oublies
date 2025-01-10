using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/MobSpawner")]

public class MobSpawner : StateBase
{
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] int nbToSpawnAtStart;
    [SerializeField] int mobMaxNb;
    public float spawnRange;
    [SerializeField] Vector2 rangeTimeBetweenSpawns;

    public override StateEntityBase PrepareEntityInstance()
    {
        MobSpawnerEntity mse = new MobSpawnerEntity();
        mse.Init(monsterPrefab, nbToSpawnAtStart, mobMaxNb, spawnRange, rangeTimeBetweenSpawns);
        return mse;
    }
}
