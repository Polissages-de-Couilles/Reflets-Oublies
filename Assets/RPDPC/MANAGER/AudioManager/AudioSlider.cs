using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioSlider : MonoBehaviour
{
    Slider slider;
    [SerializeField] AudioSettings.Volume volume;

    public void Awake()
    {
        slider = GetComponent<Slider>();
        if (GameManager.Instance.AudioManager.Mixer.GetFloat(volume.ToString(), out float v))
        {
            slider.value = Mathf.Pow(10, v);
        }
        slider.onValueChanged.AddListener(OnVolumeChange);
        if(PlayerPrefs.HasKey(volume.ToString()))
        {
            slider.value = Mathf.Pow(10, PlayerPrefs.GetFloat(volume.ToString()));
        }
        else
        {
            slider.value = 0.5f;
        }
    }

    private void OnVolumeChange(float v)
    {
        Debug.Log($"ChangeVolume : {volume.ToString()} | {v} = {Mathf.Log10(v) * 20}");
        GameManager.Instance.AudioManager.Mixer.SetFloat(volume.ToString(), Mathf.Clamp(Mathf.Log10(v) * 20, -80, 20));
        PlayerPrefs.SetFloat(volume.ToString(), v);
    }
}
