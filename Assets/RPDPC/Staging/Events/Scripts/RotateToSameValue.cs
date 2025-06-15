using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToSameValue : StagingEvent
{
    [SerializeField] Transform objectToRotate;
    //[SerializeField] float duration = 1f;

    public override void PlayEvent()
    {
        base.PlayEvent();
        if (objectToRotate == null)
        {
            DebugError("Invalid Transform");
            OnEventFinished?.Invoke();
            return;
        }
        //StartCoroutine(DoRotation());

        objectToRotate.eulerAngles = this.transform.localEulerAngles;
        Debug.Log("New Rotation : " + objectToRotate.eulerAngles);
        OnEventFinished?.Invoke();
    }

    //IEnumerator DoRotation()
    //{
    //    //yield return objectToRotate.DOLookAt(new Vector3(objectToLookAt.position.x, objectToRotate.position.y, objectToLookAt.position.z), duration).WaitForCompletion();
    //    yield return objectToRotate.DORotate(this.transform.eulerAngles, duration).WaitForCompletion();
    //    OnEventFinished?.Invoke();
    //}
}
