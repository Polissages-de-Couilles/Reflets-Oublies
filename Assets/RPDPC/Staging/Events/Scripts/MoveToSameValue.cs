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

        Vector3 pos = transform.position;

        if(useNavMesh)
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(transform.position, out myNavHit, 100, -1))
            {
                pos = myNavHit.position;
            }
        }
        
        if(objectToMove.TryGetComponent(out CharacterController controller))
        {
            StartCoroutine(MoveTo(controller, pos));
        }
        else
        {
            objectToMove.position = pos;
            OnEventFinished?.Invoke();
        }
    }

    private IEnumerator MoveTo(CharacterController controller, Vector3 pos)
    {
        controller.enabled = false;
        yield return null;
        objectToMove.transform.position = pos;
        yield return null;
        controller.enabled = true;
        OnEventFinished?.Invoke();
    }
}
