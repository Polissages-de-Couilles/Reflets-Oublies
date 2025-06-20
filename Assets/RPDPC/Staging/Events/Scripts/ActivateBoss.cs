using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : StagingEvent
{
    [SerializeField] BossStartManager bossStartManager;

    public override void PlayEvent()
    {
        base.PlayEvent();
        GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(DOActivateBoss);
        OnEventFinished?.Invoke();
    }

    private void DOActivateBoss()
    {
        GameManager.Instance.DialogueManager.EndDialogueEvent.RemoveListener(DOActivateBoss);
        bossStartManager.OnPlayerDetected();
    }
}
