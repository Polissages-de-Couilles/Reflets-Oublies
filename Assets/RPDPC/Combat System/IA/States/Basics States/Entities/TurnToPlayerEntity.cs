using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnToPlayerEntity : StateEntityBase
{
    float turnDuration;

    public override void ExitState()
    {
    }

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves, GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns, float turnDuration)
    {
        this.turnDuration = turnDuration;
    }

    public override void OnEndState()
    {
        manager.StopCoroutine(DoLookAt());
    }

    public override void OnEnterState()
    {
        manager.StartCoroutine(DoLookAt());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator DoLookAt()
    {
        yield return parent.transform.DOLookAt(player.transform.position, turnDuration).WaitForCompletion();
    }
}
