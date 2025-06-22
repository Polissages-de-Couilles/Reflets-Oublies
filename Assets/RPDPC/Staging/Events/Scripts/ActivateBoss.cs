using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : StagingEvent
{
    [SerializeField] BossStartManager bossStartManager;
    [SerializeField] Perso character;

    public override void PlayEvent()
    {
        base.PlayEvent();
        StartCoroutine(WaitABit());
    }

    private void DOActivateBoss()
    {
        GameManager.Instance.DialogueManager.EndDialogueEvent.RemoveListener(DOActivateBoss);
        bossStartManager.OnPlayerDetected();
    }

    IEnumerator WaitABit()
    {
        yield return null;
        yield return null;
        yield return null;

        if(character != Perso.None && character != Perso.Player && bossStartManager == null)
        {
            var obj = GameObject.FindGameObjectWithTag(character.ToString());
            if(obj != null && obj.TryGetComponent(out BossStartManager boss))
            {
                bossStartManager = boss;
            }
        }
        GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(DOActivateBoss);
        OnEventFinished?.Invoke();
    }
}
