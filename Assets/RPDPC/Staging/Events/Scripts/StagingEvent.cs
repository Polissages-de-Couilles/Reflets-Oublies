using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingEvent : MonoBehaviour
{
    public string ID;
    public Action OnEventFinished;
    public bool IsEventFinish { get; protected set; } = false;
    [SerializeField] bool CallFinishedEventAtStart;

    public virtual void Awake()
    {
        IsEventFinish = false;
        OnEventFinished += () => IsEventFinish = true;
    }

    public virtual void PlayEvent()
    {
        IsEventFinish = false;
        if (CallFinishedEventAtStart) OnEventFinished?.Invoke();
    }

    public void DebugError(string error)
    {
        Debug.LogError($"Error for staging event ID {ID} {GetType().Name} : " + error);
    }
}
