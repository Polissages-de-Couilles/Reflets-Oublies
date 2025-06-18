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
        SFXVolume,
        NatureVolume
    }

    public AudioMixer Mixer;
    public AudioSource SfxPrefab;
    [SerializeField] AudioClip _combatMusic;
    [SerializeField] AudioSource _otherSource;

    [SerializeField] float _musicVolume;
    AudioSource _currentMusicSource;

    [SerializeField] float _AmbienceVolume;
    AudioSource _currentAmbienceSource;

    [SerializeField] List<AudioSource> _audioSources;

    ZoneManager.Zone _currentZone;
    List<Tween> tweens = new List<Tween>();
    List<Coroutine> coroutines = new List<Coroutine>();

    bool InCombat { get; set; } = false;

    public void Awake()
    {
        _currentMusicSource = null;
        _currentAmbienceSource = null;
    }

    public IEnumerator Start()
    {
        yield return null;
        foreach (var source in _audioSources)
        {
            source.Pause();
        }
    }

    public void SwitchZone(ZoneManager.Zone zone)
    {
        _currentZone = zone;
        Debug.Log(InCombat);

        if(zone.AmbienceSource != null && (_currentAmbienceSource == null || !_currentAmbienceSource.Equals(zone.AmbienceSource)))
        {
            coroutines.Add(StartCoroutine(TransitionAudio(null, _currentAmbienceSource, zone.AmbienceSource, 2f, _AmbienceVolume)));
            _currentAmbienceSource = zone.AmbienceSource;
        }

        if (InCombat) return;
        for(int i = 0; i < coroutines.Count; i++)
        {
            if(coroutines[i] != null) StopCoroutine(coroutines[i]);
        }
        coroutines.Clear();
        for (int i = 0; i < tweens.Count; i++)
        {
            tweens[i].Pause();
            tweens[i].Kill();
        }
        tweens.Clear();
        
        if(zone.MusicSource != null && (_currentMusicSource == null || !_currentMusicSource.Equals(zone.MusicSource)))
        {
            coroutines.Add(StartCoroutine(TransitionAudio(zone.Music, _currentMusicSource, zone.MusicSource, 2f, _musicVolume)));
            _currentMusicSource = zone.MusicSource;
        }
    }

    public void EnterCombat() => EnterCombat(_combatMusic);

    public void EnterCombat(AudioClip clip)
    {
        InCombat = true;
        StopAllCoroutines();
        StartCoroutine(TransitionAudio(clip, _currentMusicSource, _otherSource, 2f, _musicVolume));
    }

    public void ExitCombat()
    {
        InCombat = false;
        SwitchZone(_currentZone);
    }

    IEnumerator TransitionAudio(AudioClip clip, AudioSource from, AudioSource to, float duration, float volume = 1f)
    {
        if(to == null) yield break;
        yield return null;
        if(from != null)
        {
            var tt = from.DOFade(0, duration * 0.5f).SetEase(Ease.Linear);
            tt.Play();
            tweens.Add(tt);
        }
        yield return new WaitForSeconds(duration * 0.5f);
        if(!to.isPlaying) to.Play();
        var t = to.DOFade(volume, duration * 0.5f).SetEase(Ease.Linear);
        tweens.Add(t);
        yield return t.WaitForCompletion();
    }

    //AudioSource GetOppsiteMusicSource(AudioSource source) => source.Equals(_musicSourceOne) ? _musicSourceTwo : _musicSourceOne;
    //AudioSource GetOppsiteAmbienceSource(AudioSource source) => source.Equals(_AmbienceSourceOne) ? _AmbienceSourceTwo : _AmbienceSourceOne;
}
