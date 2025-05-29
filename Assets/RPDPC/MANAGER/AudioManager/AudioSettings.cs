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

    public AudioMixer Mixer => _mixer;
    [SerializeField] AudioMixer _mixer;
}
