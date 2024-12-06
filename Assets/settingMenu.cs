using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class settingMenu : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;

    public GameObject OptionGameObject;
    public GameObject MenuGameObject;

    Resolution[] resolutions;
    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
    }

    public void setQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel (qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void GoMenu()
    {
        OptionGameObject.SetActive(false);
        MenuGameObject.SetActive(true);
    }
}
