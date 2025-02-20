using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayerEntity : StateEntityBase
{
    protected NavMeshAgent agent;
    protected bool isIntelligent;
    protected Vector3 lastKnownPos;

    public override void Init(bool isIntelligent)
    {
        this.isIntelligent = isIntelligent;
    }

    public override void ExitState()
    {
        onActionFinished?.Invoke();
        manager.shouldSearchStates = true;
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
        playAnim();
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
                    manager.shouldSearchStates = false;
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

    protected virtual void playAnim()
    {
        animator.Play(animationNames[0]);
    }
}
