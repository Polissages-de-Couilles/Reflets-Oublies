using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayerEntity : StateEntityBase
{
    NavMeshAgent agent;
    bool isIntelligent;
    Vector3 lastKnownPos;

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves, GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns)
    {
        this.isIntelligent = isIntelligent;
    }

    public override void ExitState()
    {
        manager.shouldSearchStates = true;
    }

    public override void OnEndState()
    {
        agent.isStopped = true;
    }

    public override void OnEnterState()
    {
        agent = parent.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
    }

    public override void OnUpdate()
    {
        if (isIntelligent) 
        {
            if (!isStateValid())
            {
                manager.shouldSearchStates = false;
                agent.SetDestination(lastKnownPos);
                if (Vector3.Distance(parent.transform.position, player.transform.position) < 2)
                {
                    ExitState();
                }
                return;
            }
            else
            {
                manager.shouldSearchStates = true;
            }
        }
        lastKnownPos = player.transform.position;
        agent.SetDestination(lastKnownPos);
    }
}
