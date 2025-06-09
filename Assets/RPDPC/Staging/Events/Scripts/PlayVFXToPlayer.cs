using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayVFXToPlayer : StagingEvent
{
    [SerializeField] GameObject vfx;
    [SerializeField] Vector3 offset;

    public override void PlayEvent()
    {
        GameObject vfxInstance = Instantiate(vfx, GameManager.Instance.Player.transform);
        vfxInstance.transform.localPosition = offset;
        OnEventFinished?.Invoke();
    }
}
