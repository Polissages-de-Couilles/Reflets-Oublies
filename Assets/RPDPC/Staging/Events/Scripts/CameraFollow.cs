using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : StagingEvent
{
    [SerializeField] Transform objectToFollow;

    public override void PlayEvent()
    {
        base.PlayEvent();
        GameManager.Instance.CamManager.VirtualCamera.Follow = objectToFollow;
        OnEventFinished?.Invoke();
    }
}
