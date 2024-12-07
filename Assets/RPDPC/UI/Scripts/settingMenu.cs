using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PDC.Localization;

public class settingMenu : UIPanel
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown languageDropdown;

    public GameObject OptionGameObject;
    public GameObject MenuGameObject;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentRes = 0;
        for (int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentRes = i;
            }
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentRes;

        languageDropdown.ClearOptions();
        List<string> languageOption = new List<string>();
        for (int i = 0; i < LocalizationManager._languages.Count; i++)
        {
            languageOption.Add(LocalizationManager._languages[i]);
        }
        languageDropdown.AddOptions(languageOption);
        languageDropdown.value = LocalizationManager.languageID;
        languageDropdown.onValueChanged.AddListener(GameManager.Instance.LocalizationManager.ChangeLanguage);
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
