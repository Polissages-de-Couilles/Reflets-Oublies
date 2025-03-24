using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingEvent : MonoBehaviour
{
    public string ID;
    public Action OnEventFinished;

    public virtual void PlayEvent()
    {
        OnEventFinished?.Invoke();
    }

    public enum StagingEventTypes
    {
        MoveToME
    }
}
