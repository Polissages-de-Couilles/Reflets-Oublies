using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderVolumeLevel : MonoBehaviour
{
    public Slider thisSlider;
    public float masterVolume;
    public float vfxVolume;
    public float voiceVolume;
    public float musicVolume;

    public void SetSpecificVolume(string whatValue)
    {
        float sliderValue = thisSlider.value;

        if (whatValue == "Master")
        {
            masterVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MasterVolume", masterVolume);
        }

        if (whatValue == "Music")
        {
            musicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
        }

        if (whatValue == "VFX")
        {
            vfxVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("vfxVolume", vfxVolume);
        }

        if (whatValue == "Voice")
        {
            voiceVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("VoiceVolume", voiceVolume);
        }
    }
}
