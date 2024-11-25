using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingEntity : StateEntityBase
{
    public override void ExitState()
    {
    }

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves, GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns)
    {
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
        manager.shouldSearchStates = false;
    }

    public override void OnUpdate()
    {
        if (!isStateValid())
        {
            manager.shouldSearchStates = true;
        }
    }
}
