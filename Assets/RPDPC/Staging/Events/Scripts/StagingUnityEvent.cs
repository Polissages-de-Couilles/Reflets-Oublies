using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StagingUnityEvent : StagingEvent
{
    [SerializeField] UnityEvent _event;
    public override void PlayEvent()
    {
        base.PlayEvent();
        _event?.Invoke();
        OnEventFinished?.Invoke();
    }
}
