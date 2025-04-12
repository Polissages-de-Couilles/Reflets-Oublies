using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToME : StagingEvent
{
    [SerializeField] NavMeshAgent objectToMove;
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    float oldSpeed;


    public override void PlayEvent()
    {
        base.PlayEvent();
        if (objectToMove == null)
        {
            DebugError("Invalid Agent");
            OnEventFinished?.Invoke();
            return;
        }

        NavMeshPath navMeshPath = new NavMeshPath();
        if (objectToMove.CalculatePath(transform.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            oldSpeed = objectToMove.speed;
            objectToMove.speed = speed;
            objectToMove.SetPath(navMeshPath);
            StartCoroutine(CheckIfDestinationReached());
        }
        else
        {
            DebugError("Destination is unreachable for this agent");
            OnEventFinished?.Invoke();
        }
    }

    IEnumerator CheckIfDestinationReached()
    {
        while (!pathComplete())
        {
            yield return null;
        }
        objectToMove.speed = oldSpeed;
        OnEventFinished?.Invoke();
    }

    protected bool pathComplete()
    {
        if (Vector3.Distance(objectToMove.destination, objectToMove.transform.position) <= stoppingDistance)
        {
            if (!objectToMove.hasPath || objectToMove.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }

}
