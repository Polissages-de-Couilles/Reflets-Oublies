using DG.Tweening;
using System;
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

    [SerializeField] List<AudioSource> _ambienceSources;
    [SerializeField] List<AudioSource> _musicSources;

    ZoneManager.Zone _currentZone;
    List<Tween> tweensMusic = new List<Tween>();
    List<Tween> tweensAmbiance = new List<Tween>();
    List<Coroutine> coroutinesAmbiance = new List<Coroutine>();
    List<Coroutine> coroutinesMusic = new List<Coroutine>();
    List<Coroutine> coroutinesCombat = new List<Coroutine>();
    StateManager playerStateManager;

    public bool InCombat { get; private set; } = false;
    bool lastMusicWasCombat = false;

    public void Awake()
    {
        _currentMusicSource = null;
        _currentAmbienceSource = null;
    }

    public IEnumerator Start()
    {
        yield return null;
        foreach (var source in _ambienceSources)
        {
            source.Pause();
        }
        foreach(var source in _musicSources)
        {
            source.Pause();
        }
        if(GameManager.Instance.Player != null && GameManager.Instance.Player.TryGetComponent(out StateManager manager))
        {
            playerStateManager = manager;
            manager.OnFightStateChanged += OnCombatEnter;
        }

        var volumeNames = Enum.GetNames(typeof(Volume));
        foreach (var volumeName in volumeNames)
        {
            Debug.Log(volumeName);
            if(PlayerPrefs.HasKey(volumeName))
            {
                GameManager.Instance.AudioManager.Mixer.SetFloat(volumeName, Mathf.Clamp(Mathf.Log10(PlayerPrefs.GetFloat(volumeName)) * 20, -80, 20));
            }
            else
            {
                GameManager.Instance.AudioManager.Mixer.SetFloat(volumeName, Mathf.Clamp(Mathf.Log10(0.5f) * 20, -80, 20));
                PlayerPrefs.SetFloat(volumeName, 0.5f);
            }
        }

        //while(GameManager.Instance.Player != null)
        //{
        //    yield return new WaitForSeconds(2f);
        //    if(!playerStateManager.IsHostileEnemies && _otherSource.volume > 0) ForceExitCombat();
        //}
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
            coroutinesAmbiance.Add(StartCoroutine(TransitionAudio(null, _ambienceSources, zone.AmbienceSource, 2f, _AmbienceVolume)));
            _currentAmbienceSource = zone.AmbienceSource;
        }

        //if (InCombat && !ignoreCombat) return;

        foreach(var coroutine in coroutinesCombat)
        {
            if(coroutine == null) continue;
            StopCoroutine(coroutine);
        }
        coroutinesCombat.Clear();

        for(int i = 0; i < coroutinesMusic.Count; i++)
        {
            if(coroutinesMusic[i] != null) StopCoroutine(coroutinesMusic[i]);
        }
        coroutinesMusic.Clear();

        //if(!ignoreCombat && lastMusicWasCombat)
        //{
        //    lastMusicWasCombat = false;
        //    for(int i = 0; i < tweensMusic.Count; i++)
        //    {
        //        tweensMusic[i].Complete();
        //    }
        //}
        for (int i = 0; i < tweensMusic.Count; i++)
        {
            tweensMusic[i].Pause();
            tweensMusic[i].Kill();
        }
        tweensMusic.Clear();
        
        if(zone.MusicSource != null && (_currentMusicSource == null || !_currentMusicSource.Equals(zone.MusicSource)))
        {
            _currentMusicSource = zone.MusicSource;
            coroutinesMusic.Add(StartCoroutine(TransitionAudio(zone.Music, _musicSources, zone.MusicSource, 2f, _musicVolume, true)));
        }
    }

    private void OnCombatEnter(bool combat)
    {
        Debug.Log("Combat : " +  combat);
        if(combat)
        {
            //EnterCombat();
        }

        if(!combat)
        {
            //ExitCombat();
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
        _currentMusicSource = _otherSource;
        StartCoroutine(TransitionAudio(clip, _musicSources, _otherSource, 1f, volume, true));
    }

    public void ExitCombat()
    {
        coroutinesCombat.Add(StartCoroutine(WaitBeforeExitCombat()));
    }

    public void ForceExitCombat()
    {
        //InCombat = false;
        SwitchZone(_currentZone, true);
        //lastMusicWasCombat = true;
    }

    IEnumerator WaitBeforeExitCombat()
    {
        yield return new WaitForSeconds(2f);
        if(!playerStateManager.IsHostileEnemies && InCombat)
        {
            InCombat = false;
            SwitchZone(_currentZone, true);
            lastMusicWasCombat = true;
        }
    }

    IEnumerator TransitionAudio(AudioClip clip, List<AudioSource> from, AudioSource to, float duration, float volume = 1f, bool isMusic = false)
    {
        if(to == null) yield break;
        yield return null;

        foreach(var source in from)
        {
            var tt = source.DOFade(0, duration * 0.5f).SetEase(Ease.Linear).OnComplete(() => source.volume = 0);
            tt.Play();
            if(isMusic) tweensMusic.Add(tt);
            else tweensAmbiance.Add(tt);
        }

        //if(from != null)
        //{
        //    var tt = from.DOFade(0, duration * 0.5f).SetEase(Ease.Linear).OnComplete(() => from.volume = 0);
        //    tt.Play();
        //    if(isMusic) tweensMusic.Add(tt);
        //    else tweensAmbiance.Add(tt);
        //}
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

    [SerializeField] List<AudioClip> _deathMonstres;
    [SerializeField] float _deathVolume;

    public void PlayDeathBot()
    {
        var clip = _deathMonstres[UnityEngine.Random.Range(0, _deathMonstres.Count)];
        var sfx = Instantiate(SfxPrefab);
        sfx.clip = clip;
        sfx.volume = _deathVolume;
        sfx.Play();
        Destroy(sfx, clip.length);
    }
}
