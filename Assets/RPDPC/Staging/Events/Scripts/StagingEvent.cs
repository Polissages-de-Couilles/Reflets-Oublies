using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingEvent : MonoBehaviour
{
    public string ID;
    public StagingEventTypes Type;
    public Action OnEventFinished;
    [SerializeField] bool CallFinishedEventAtStart;

    public virtual void PlayEvent()
    {
        if(CallFinishedEventAtStart) OnEventFinished?.Invoke();
    }

    public enum StagingEventTypes
    {
        MoveToME,
        PlayAnimatorState,
        PlayVFXHere,
        RotateToME
    }

    public void DebugError(string error)
    {
        Debug.LogError($"Error for staging event ID {ID} {GetType().Name} : " + error);
    }
}
