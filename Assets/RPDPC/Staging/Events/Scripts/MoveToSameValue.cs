using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSameValue : StagingEvent
{
    [SerializeField] Transform objectToMove;
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

        objectToMove.position = this.transform.position;
        OnEventFinished?.Invoke();
    }
}
