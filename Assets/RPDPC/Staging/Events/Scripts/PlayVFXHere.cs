using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayVFXHere : StagingEvent
{
    GameObject vfx;

    public override void PlayEvent()
    {
        GameObject vfxInstance = Instantiate(vfx, transform);
        vfxInstance.transform.localPosition = transform.localPosition;
        OnEventFinished?.Invoke();
    }
}
