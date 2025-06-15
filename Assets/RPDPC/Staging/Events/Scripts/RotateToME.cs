using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateToME : StagingEvent
{
    [SerializeField] Transform objectToRotate;
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
        yield return objectToRotate.DODynamicLookAt(new Vector3(transform.position.x, objectToRotate.position.y, transform.position.z), duration, AxisConstraint.Y).WaitForCompletion();
        OnEventFinished?.Invoke();
    }
}
