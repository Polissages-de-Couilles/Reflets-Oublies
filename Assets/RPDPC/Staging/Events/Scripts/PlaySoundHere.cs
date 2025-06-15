using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundHere : StagingEvent
{
    [SerializeField] AudioClip _clip;

    public override void PlayEvent()
    {
        base.PlayEvent();

        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        yield return null;
        AudioSource.PlayClipAtPoint(_clip, this.transform.position);
        yield return new WaitForSeconds(_clip.length);
        OnEventFinished?.Invoke();
    }
}
