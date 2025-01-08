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
    bool isMoving => Vector3.Distance(currentDestination, parent.transform.position) >= 0.1f;
    Vector3 currentDestination;

    public override void Init(Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves)
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
        if (agent.isStopped)
            agent.isStopped = false;
        isPosReached = false;
        goToRandomPos();
    }

    public override void OnUpdate()
    {
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
                if (!isMoving)
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
        isPosReached = true;
        animator.Play(animationNames[0]);
        yield return new WaitForSeconds(Random.Range(rangeWaitBetweenMoves.x, rangeWaitBetweenMoves.y));
        manager.shouldSearchStates = true;
        timerRunning = false;
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
            animator.Play(animationNames[1]);
            agent.SetDestination(currentDestination);
        }
        else 
        {
            goToRandomPos();
        }
    }
}
