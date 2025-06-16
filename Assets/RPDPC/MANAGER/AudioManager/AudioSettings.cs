using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public enum Volume
    {
        MasterVolume,
        MusicVolume,
        VoiceVolume,
        SFXVolume
    }

    public AudioMixer Mixer;
    public AudioSource SfxPrefab;
    [SerializeField] AudioClip _combatMusic;

    [SerializeField] AudioSource _musicSourceOne;
    [SerializeField] AudioSource _musicSourceTwo;
    AudioSource _currentMusicSource;

    [SerializeField] AudioSource _ambianceSourceOne;
    [SerializeField] AudioSource _ambianceSourceTwo;
    AudioSource _currentAmbianceSource;

    ZoneManager.Zone _currentZone;

    bool InCombat { get; set; } = false;

    public void SwitchZone(ZoneManager.Zone zone)
    {
        _currentZone = zone;
        if (InCombat) return;
        StopAllCoroutines();
        if (zone.Music != null) StartCoroutine(TransitionAudio(zone.Music, _currentMusicSource, GetOppsiteMusicSource(_currentMusicSource), 2f, 0.5f));
        if (zone.AmbianceSound != null) StartCoroutine(TransitionAudio(zone.AmbianceSound, _currentAmbianceSource, GetOppsiteAmbianceSource(_currentAmbianceSource), 2f, 0.2f));
    }

    public void EnterCombat() => EnterCombat(_combatMusic);

    public void EnterCombat(AudioClip clip)
    {
        InCombat = true;
        StopAllCoroutines();
        StartCoroutine(TransitionAudio(clip, _currentMusicSource, GetOppsiteMusicSource(_currentMusicSource), 1f, 0.5f));
    }

    public void ExitCombat()
    {
        InCombat = false;
        SwitchZone(_currentZone);
    }

    IEnumerator TransitionAudio(AudioClip clip, AudioSource from, AudioSource to, float duration, float volume = 1f)
    {
        yield return null;
        if(from != null)
        {
            from.DOFade(0, duration);
        }
        to.clip = clip;
        yield return to.DOFade(volume, duration).WaitForCompletion();
    }

    AudioSource GetOppsiteMusicSource(AudioSource source) => source.Equals(_musicSourceOne) ? _musicSourceTwo : _musicSourceOne;
    AudioSource GetOppsiteAmbianceSource(AudioSource source) => source.Equals(_ambianceSourceOne) ? _ambianceSourceTwo : _ambianceSourceOne;
}
