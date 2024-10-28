using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Game/IA/States/Base/RandomMoveInRange")]
public class RandomMoveInRange : StateBase
{
    [SerializeField] Vector3 randomCenter;
    [SerializeField] float randomRange;
    [SerializeField] bool shouldOnlyMoveOnce = false;
    [SerializeField] bool WaitForMoveToFinishBeforeEndOrSwitchingState = false;
    StateMachineManager manager;
    GameObject parent;
    NavMeshAgent agent;
    bool isPosReached;
    bool timerRunning = false;
    Vector3 currentDestination;
    [SerializeField] Vector2 rangeWaitBetweenMoves;

    public override void Init(StateMachineManager manager, GameObject parent, GameObject player)
    {
        this.manager = manager;
        this.parent = parent;
        agent = parent.GetComponent<NavMeshAgent>();
    }

    public override void ExitState()
    {
        manager.shouldSearchStates = true;
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
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
        Vector3 randomPoint = randomCenter + Random.insideUnitSphere * randomRange;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
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
