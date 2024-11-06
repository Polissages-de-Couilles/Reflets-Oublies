using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Game/IA/States/Base/FollowPlayer")]
public class FollowPlayer : StateBase
{
    GameObject player;
    GameObject parent;
    NavMeshAgent agent;
    StateMachineManager manager;
    [Tooltip("Does the bot have to go to the last known place even if the conditions are not met anymore")]
    [SerializeField] bool isIntelligent;
    Vector3 lastKnownPos;

    public override void Init(StateMachineManager manager, GameObject parent, GameObject player)
    {
        agent = parent.GetComponent<NavMeshAgent>();
        this.player = player;
        this.parent = parent;
        this.manager = manager;
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
