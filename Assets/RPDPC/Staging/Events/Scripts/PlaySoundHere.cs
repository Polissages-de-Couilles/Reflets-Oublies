using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundHere : StagingEvent
{
    [SerializeField] AudioClip _clip;
    [SerializeField] float _volume = 0.1f;

    public override void PlayEvent()
    {
        base.PlayEvent();

        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        yield return null;
        var audio = Instantiate(GameManager.Instance.AudioManager.SfxPrefab, this.transform.position, Quaternion.identity, this.transform);
        audio.transform.localPosition = Vector3.zero;
        audio.volume = _volume;
        audio.clip = _clip;
        audio.Play();
        yield return new WaitForSeconds(_clip.length);
        Destroy(audio.gameObject);
        OnEventFinished?.Invoke();
    }
}
