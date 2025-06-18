using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObject : StagingEvent
{
    [SerializeField] GameObject[] objectsToSet;
    [SerializeField] bool isActive;

    public override void PlayEvent()
    {
        base.PlayEvent();
        foreach (var obj in objectsToSet)
        {
            obj.SetActive(isActive);
        }
        OnEventFinished?.Invoke();
    }
}
