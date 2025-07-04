using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowListOfPositionsEntity : StateEntityBase
{
    List<Vector3> positions;
    bool loop;

    NavMeshAgent agent;

    public override void ExitState()
    {
    }

    public override void Init(List<Vector3> positions, bool loop)
    {
        this.positions = positions;
        this.loop = loop;
    }

    public override void OnEndState()
    {
        agent.isStopped = true;
        manager.StopCoroutine(FollowAllPos());
    }

    public override void OnEnterState()
    {
        agent = parent.GetComponent<NavMeshAgent>();
        if (agent.isStopped)
            agent.isStopped = false;

        manager.StartCoroutine(FollowAllPos());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator FollowAllPos() 
    {
        if (loop) 
        {
            while (true)
            {
                foreach (Vector3 pos in positions)
                {
                    agent.SetDestination(pos);
                    while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                    {
                        yield return null;
                    }
                }
            }
        }
        else
        {
            foreach (Vector3 pos in positions)
            {
                agent.SetDestination(pos);
                while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null;
                }
            }
        }
    }
}
