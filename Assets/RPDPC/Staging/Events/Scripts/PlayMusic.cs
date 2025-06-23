using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : StagingEvent
{
    [SerializeField] AudioClip musicClip;
    [SerializeField] float musicVolume;
    [SerializeField] bool _returnToPreviousMusic = false;

    public override void PlayEvent()
    {
        base.PlayEvent();

        if(_returnToPreviousMusic)
        {
            GameManager.Instance.AudioManager.ForceExitCombat();
        }
        else
        {
            GameManager.Instance.AudioManager.EnterCombat(musicClip, musicVolume);
        }
        StartCoroutine(WaitAFrame());
    }

    IEnumerator WaitAFrame()
    {
        yield return null;
        OnEventFinished?.Invoke();
    }
}
