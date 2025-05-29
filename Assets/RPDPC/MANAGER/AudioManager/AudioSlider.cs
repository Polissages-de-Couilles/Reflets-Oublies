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
        if(GameManager.Instance.AudioManager.Mixer.GetFloat(volume.ToString(), out float v))
        {
            slider.value = Mathf.Pow(10, v);
        }
        slider.onValueChanged.AddListener(OnVolumeChange);
    }

    private void OnVolumeChange(float v)
    {
        GameManager.Instance.AudioManager.Mixer.SetFloat(volume.ToString(), Mathf.Log10(v) * 20);
    }
}
