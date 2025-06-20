using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : StagingEvent
{
    [SerializeField] BossStartManager bossStartManager;

    public override void PlayEvent()
    {
        base.PlayEvent();
        bossStartManager.OnPlayerDetected();
        OnEventFinished?.Invoke();
    }
}
