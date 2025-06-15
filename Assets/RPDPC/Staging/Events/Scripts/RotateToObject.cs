using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToObject : StagingEvent
{
    [SerializeField] Transform objectToRotate;
    [SerializeField] Transform objectToLookAt;
    [SerializeField] float duration = 1f;

    public override void PlayEvent()
    {
        base.PlayEvent();
        if (objectToRotate == null)
        {
            DebugError("Invalid Transform");
            OnEventFinished?.Invoke();
            return;
        }
        StartCoroutine(DoRotation());
    }

    IEnumerator DoRotation()
    {
        //yield return objectToRotate.DOLookAt(new Vector3(objectToLookAt.position.x, objectToRotate.position.y, objectToLookAt.position.z), duration).WaitForCompletion();
        yield return objectToRotate.DODynamicLookAt(new Vector3(objectToLookAt.position.x, objectToRotate.position.y, objectToLookAt.position.z), duration, AxisConstraint.Y).WaitForCompletion();
        OnEventFinished?.Invoke();
    }
}
