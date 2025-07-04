using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayerEntity : StateEntityBase
{
    protected NavMeshAgent agent;
    protected bool isIntelligent;
    protected Vector3 lastKnownPos;
    protected bool shouldStopWhenNear;
    protected float stopNearDistance;
    protected bool shouldStopWhenFar;
    protected float stopFarDistance;

    public override void Init(bool isIntelligent, bool shouldStopWhenNear, float stopNearDistance, bool shouldStopWhenFar, float stopFarDistance)
    {
        this.isIntelligent = isIntelligent;
        this.shouldStopWhenNear = shouldStopWhenNear;
        this.stopNearDistance = stopNearDistance;
        this.shouldStopWhenFar = shouldStopWhenFar;
        this.stopFarDistance = stopFarDistance;
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

        if (shouldStopWhenNear)
        {
            if (Vector3.Distance(parent.transform.position, player.transform.position) < stopNearDistance)
            {
                ExitState();
            }
        }

        if (shouldStopWhenFar)
        {
            if (Vector3.Distance(parent.transform.position, player.transform.position) > stopFarDistance)
            {
                ExitState();
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
