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
    List<Tween> tweensMusic = new List<Tween>();
    List<Tween> tweensAmbiance = new List<Tween>();
    List<Coroutine> coroutinesAmbiance = new List<Coroutine>();
    List<Coroutine> coroutinesMusic = new List<Coroutine>();

    bool InCombat { get; set; } = false;
    bool inCombat = true;
    bool lastMusicWasCombat = false;

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
        if(GameManager.Instance.Player.TryGetComponent(out StateManager manager))
        {
            manager.OnFightStateChanged += OnCombatEnter;
        }
    }

    public void SwitchZone(ZoneManager.Zone zone, bool ignoreCombat = false)
    {
        _currentZone = zone;
        Debug.Log(InCombat);

        for(int i = 0; i < coroutinesAmbiance.Count; i++)
        {
            if(coroutinesAmbiance[i] != null) StopCoroutine(coroutinesAmbiance[i]);
        }
        coroutinesAmbiance.Clear();
        for(int i = 0; i < tweensAmbiance.Count; i++)
        {
            tweensAmbiance[i].Pause();
            tweensAmbiance[i].Kill();
        }
        tweensAmbiance.Clear();
        if(zone.AmbienceSource != null && (_currentAmbienceSource == null || !_currentAmbienceSource.Equals(zone.AmbienceSource)))
        {
            coroutinesAmbiance.Add(StartCoroutine(TransitionAudio(null, _currentAmbienceSource, zone.AmbienceSource, 2f, _AmbienceVolume)));
            _currentAmbienceSource = zone.AmbienceSource;
        }

        if (InCombat && !ignoreCombat) return;
        for(int i = 0; i < coroutinesMusic.Count; i++)
        {
            if(coroutinesMusic[i] != null) StopCoroutine(coroutinesMusic[i]);
        }
        coroutinesMusic.Clear();

        if(!ignoreCombat && lastMusicWasCombat)
        {
            lastMusicWasCombat = false;
            for(int i = 0; i < tweensMusic.Count; i++)
            {
                tweensMusic[i].Complete();
            }
        }
        for (int i = 0; i < tweensMusic.Count; i++)
        {
            tweensMusic[i].Pause();
            tweensMusic[i].Kill();
        }
        tweensMusic.Clear();
        
        if(zone.MusicSource != null && (_currentMusicSource == null || !_currentMusicSource.Equals(zone.MusicSource)))
        {
            coroutinesMusic.Add(StartCoroutine(TransitionAudio(zone.Music, _currentMusicSource, zone.MusicSource, 2f, _musicVolume, true)));
            _currentMusicSource = zone.MusicSource;
        }
    }

    private void OnCombatEnter(bool combat)
    {
        Debug.Log("Combat : " +  combat);
        inCombat = combat;
        if(combat && !InCombat)
        {
            EnterCombat();
        }

        if(!combat)
        {
            ExitCombat();
        }
    }

    public void EnterCombat() => EnterCombat(_combatMusic, _musicVolume);

    public void EnterCombat(AudioClip clip, float volume)
    {
        InCombat = true;
        for(int i = 0; i < coroutinesMusic.Count; i++)
        {
            if(coroutinesMusic[i] != null) StopCoroutine(coroutinesMusic[i]);
        }
        coroutinesMusic.Clear();
        for(int i = 0; i < tweensMusic.Count; i++)
        {
            tweensMusic[i].Pause();
            tweensMusic[i].Kill();
        }
        tweensMusic.Clear();
        StartCoroutine(TransitionAudio(clip, _currentMusicSource, _otherSource, 1f, volume, true));
        _currentMusicSource = _otherSource;
    }

    public void ExitCombat()
    {
        if(!InCombat) return;
        InCombat = false;
        StartCoroutine(WaitBeforeExitCombat());
    }

    public void ForceExitCombat()
    {
        InCombat = false;
        SwitchZone(_currentZone, true);
        lastMusicWasCombat = true;
    }

    IEnumerator WaitBeforeExitCombat()
    {
        yield return new WaitForSeconds(2f);
        InCombat = inCombat;
        if(!inCombat)
        {
            SwitchZone(_currentZone, true);
            lastMusicWasCombat = true;
        }
    }

    IEnumerator TransitionAudio(AudioClip clip, AudioSource from, AudioSource to, float duration, float volume = 1f, bool isMusic = false)
    {
        if(to == null) yield break;
        yield return null;
        if(from != null)
        {
            var tt = from.DOFade(0, duration * 0.5f).SetEase(Ease.Linear).OnComplete(() => from.volume = 0);
            tt.Play();
            if(isMusic) tweensMusic.Add(tt);
            else tweensAmbiance.Add(tt);
        }
        yield return new WaitForSeconds(duration * 0.5f);
        if(clip != null) to.clip = clip;
        if(!to.isPlaying) to.Play();
        var t = to.DOFade(volume, duration * 0.5f).SetEase(Ease.Linear).OnComplete(() => to.volume = volume);
        if(isMusic) tweensMusic.Add(t);
        else tweensAmbiance.Add(t);
        yield return t.WaitForCompletion();
    }

    //AudioSource GetOppsiteMusicSource(AudioSource source) => source.Equals(_musicSourceOne) ? _musicSourceTwo : _musicSourceOne;
    //AudioSource GetOppsiteAmbienceSource(AudioSource source) => source.Equals(_AmbienceSourceOne) ? _AmbienceSourceTwo : _AmbienceSourceOne;
}
