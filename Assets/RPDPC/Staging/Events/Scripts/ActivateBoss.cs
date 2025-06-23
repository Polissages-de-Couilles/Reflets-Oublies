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
        if(character != Perso.None && character != Perso.Player && bossStartManager == null)
        {
            var obj = GameManager.Instance.StoryManager._perso[character];
            if(obj != null && obj.TryGetComponent(out BossStartManager boss))
            {
                bossStartManager = boss;
            }
        }
        Debug.Log("Set Active Boss");
        GameManager.Instance.DialogueManager.OnEndDialogue += DOActivateBoss;
        OnEventFinished?.Invoke();
    }

    private void DOActivateBoss()
    {
        StartCoroutine(WaitABit());
    }

    IEnumerator WaitABit()
    {
        yield return null;

        Debug.Log("Staging Active Boss");

        GameManager.Instance.DialogueManager.OnEndDialogue -= DOActivateBoss;
        bossStartManager.OnPlayerDetected();
    }
}
