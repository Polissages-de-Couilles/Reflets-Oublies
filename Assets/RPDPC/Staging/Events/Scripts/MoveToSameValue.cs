using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToSameValue : StagingEvent
{
    [SerializeField] Transform objectToMove;
    [SerializeField] bool useNavMesh = false;
    //[SerializeField] float duration = 1f;

    public override void PlayEvent()
    {
        base.PlayEvent();
        if (objectToMove == null)
        {
            DebugError("Invalid Transform");
            OnEventFinished?.Invoke();
            return;
        }

        if(useNavMesh)
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(transform.position, out myNavHit, 100, -1))
            {
                objectToMove.transform.position = myNavHit.position;
            }
        }
        else
        {
            objectToMove.position = this.transform.position;
        }

        OnEventFinished?.Invoke();
    }
}
