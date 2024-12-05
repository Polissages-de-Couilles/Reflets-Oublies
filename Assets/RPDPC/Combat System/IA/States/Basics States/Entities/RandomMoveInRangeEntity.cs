using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMoveInRangeEntity : StateEntityBase
{
    Vector3 searchCenter;
    float searchRange;
    bool shouldOnlyMoveOnce;
    bool WaitForMoveToFinishBeforeEndOrSwitchingState;
    Vector2 rangeWaitBetweenMoves;
    NavMeshAgent agent;
    bool isPosReached;
    bool timerRunning = false;
    Vector3 currentDestination;

    public override void Init(bool isIntelligent, List<SOAttack.AttackDetails> attacks, List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves, GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns)
    {
        this.searchCenter = searchCenter;
        this.searchRange = searchRange;
        this.shouldOnlyMoveOnce = shouldOnlyMoveOnce;
        this.WaitForMoveToFinishBeforeEndOrSwitchingState = WaitForMoveToFinishBeforeEndOrSwitchingState;
        this.rangeWaitBetweenMoves = rangeWaitBetweenMoves;
    }

    public override void ExitState()
    {
        manager.shouldSearchStates = true;
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
        agent.isStopped = true;
    }

    public override void OnEnterState()
    {
        agent = parent.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        isPosReached = false;
        goToRandomPos();
    }

    public override void OnUpdate()
    {
        Debug.DrawLine(parent.transform.forward, parent.transform.forward * 1000, Color.red, 2f);
        if (!timerRunning)
        {
            if (isPosReached)
            {
                if (!shouldOnlyMoveOnce)
                {
                    goToRandomPos();
                }
                else
                {
                    ExitState();
                }
            }
            else
            {
                if (agent.velocity == Vector3.zero)
                {
                    if (!shouldOnlyMoveOnce)
                    {
                        manager.StartCoroutine(resetRandomMove());
                    }

                }
            }
        }
    }

    public IEnumerator resetRandomMove() 
    {
        timerRunning = true;
        yield return new WaitForSeconds(Random.Range(rangeWaitBetweenMoves.x, rangeWaitBetweenMoves.y));
        manager.shouldSearchStates = true;
        timerRunning = false;
        isPosReached = true;
    }

    private void goToRandomPos()
    {
        Vector3 randomPoint = searchCenter + Random.insideUnitSphere * searchRange;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, searchRange, NavMesh.AllAreas))
        {   
            if (WaitForMoveToFinishBeforeEndOrSwitchingState)
            {
                manager.shouldSearchStates = false;
            }
            isPosReached = false;
            currentDestination = hit.position;
            agent.SetDestination(currentDestination);
        }
        else 
        {
            goToRandomPos();
        }
    }
}
