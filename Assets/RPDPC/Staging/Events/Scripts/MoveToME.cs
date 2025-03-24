using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToME : StagingEvent
{
    [SerializeField] NavMeshAgent objectToMove;

    public override void PlayEvent()
    {
        if (objectToMove == null)
        {
            Debug.LogError($"Error for staging event ID {ID} {GetType().Name} : Invalid Agent");
            OnEventFinished?.Invoke();
        }

        NavMeshPath navMeshPath = new NavMeshPath();
        if (objectToMove.CalculatePath(transform.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            objectToMove.SetPath(navMeshPath);
            StartCoroutine(CheckIfDestinationReached());
        }
        else
        {
            Debug.LogError($"Error for staging event ID {ID} {GetType().Name} : Destination is unreachable for this agent");
            OnEventFinished?.Invoke();
        }
    }

    IEnumerator CheckIfDestinationReached()
    {
        while (!pathComplete())
        {
            yield return null;
        }
        OnEventFinished?.Invoke();
    }

    protected bool pathComplete()
    {
        if (Vector3.Distance(objectToMove.destination, objectToMove.transform.position) <= objectToMove.stoppingDistance)
        {
            if (!objectToMove.hasPath || objectToMove.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }

}
