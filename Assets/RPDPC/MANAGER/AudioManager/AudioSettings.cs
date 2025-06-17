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

    [SerializeField] float _musicVolume;
    [SerializeField] AudioSource _musicSourceOne;
    [SerializeField] AudioSource _musicSourceTwo;
    AudioSource _currentMusicSource;

    [SerializeField] float _ambianceVolume;
    [SerializeField] AudioSource _ambianceSourceOne;
    [SerializeField] AudioSource _ambianceSourceTwo;
    AudioSource _currentAmbianceSource;

    ZoneManager.Zone _currentZone;
    List<Tween> tweens = new List<Tween>();

    bool InCombat { get; set; } = false;

    public void Awake()
    {
        _currentMusicSource = _musicSourceTwo;
        _currentAmbianceSource = _ambianceSourceTwo;
    }

    public void SwitchZone(ZoneManager.Zone zone)
    {
        _currentZone = zone;
        Debug.Log(InCombat);
        if (InCombat) return;
        StopAllCoroutines();
        for (int i = 0; i < tweens.Count; i++)
        {
            tweens[i].Pause();
            tweens[i].Kill();
        }
        tweens.Clear();
        if(zone.AmbianceSound != null)
        {
            StartCoroutine(TransitionAudio(zone.AmbianceSound, _currentAmbianceSource, GetOppsiteAmbianceSource(_currentAmbianceSource), 2f, _ambianceVolume));
            _currentAmbianceSource = GetOppsiteAmbianceSource(_currentAmbianceSource);
        }
        if (zone.Music != null)
        {
            StartCoroutine(TransitionAudio(zone.Music, _currentMusicSource, GetOppsiteMusicSource(_currentMusicSource), 2f, _musicVolume));
            _currentMusicSource = GetOppsiteMusicSource(_currentMusicSource);
        }
    }

    public void EnterCombat() => EnterCombat(_combatMusic);

    public void EnterCombat(AudioClip clip)
    {
        InCombat = true;
        StopAllCoroutines();
        StartCoroutine(TransitionAudio(clip, _currentMusicSource, GetOppsiteMusicSource(_currentMusicSource), 2f, _musicVolume));
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
            var tt = from.DOFade(0, duration * 0.75f).SetEase(Ease.Linear);
            tt.Play();
            tweens.Add(tt);
        }
        yield return new WaitForSeconds(duration * 0.25f);
        to.clip = clip;
        to.Play();
        var t = to.DOFade(volume, duration * 0.75f).SetEase(Ease.Linear);
        tweens.Add(t);
        yield return t.WaitForCompletion();
    }

    AudioSource GetOppsiteMusicSource(AudioSource source) => source.Equals(_musicSourceOne) ? _musicSourceTwo : _musicSourceOne;
    AudioSource GetOppsiteAmbianceSource(AudioSource source) => source.Equals(_ambianceSourceOne) ? _ambianceSourceTwo : _ambianceSourceOne;
}
