using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAwayFromPlayerEntity : StateEntityBase
{
    NavMeshAgent agent;

    public override void ExitState()
    {
    }

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves, GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns, float turnDuration)
    {
    }

    public override void OnEndState()
    {
        agent.isStopped = true;
    }

    public override void OnEnterState()
    {
        agent = parent.GetComponent<NavMeshAgent>();
        if (agent.isStopped)
            agent.isStopped = false;
    }

    public override void OnUpdate()
    {
        //NavMeshHit hit;
        //if (NavMesh.SamplePosition(parent.transform.position - (player.transform.position - parent.transform.position).normalized, out hit, Mathf.Infinity, NavMesh.AllAreas))
        //{
        //    agent.SetDestination(hit.position);
        //}
        agent.SetDestination(parent.transform.position - (player.transform.position - parent.transform.position).normalized);
    }
}
