using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAwayFromPlayerEntity : StateEntityBase
{
    NavMeshAgent agent;
    bool keepWatchingPlayer;

    public override void ExitState()
    {
    }

    public override void Init(bool isIntelligent)
    {
        keepWatchingPlayer = isIntelligent;
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
        agent.SetDestination(parent.transform.position - (player.transform.position - parent.transform.position).normalized);
        if (keepWatchingPlayer)
            agent.transform.LookAt(player.transform.position);
    }
}
