using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using PDC.Localization;
using TMPro;

public class MainMenuManager : UIPanel
{
    public string GameSceneName;

    public GameObject UIOption;
    public GameObject UIMenu;
    public GameObject UILoading;
    public TextMeshProUGUI LoadingText;

    protected override void Start()
    {
        base.Start();
        LocalizationManager.OnLocaReady(() =>
        {
            UILoading.SetActive(false);
        });

        StartCoroutine(TextAnim());
    }

    IEnumerator TextAnim()
    {
        yield return null;
        LoadingText.text = "Loading ";
        while(!LocalizationManager.IsLocaReady)
        {
            for(int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.5f);
                var text = "Loading ";
                for(int y = 0; y < i + 1; y++)
                {
                    text += ".";
                }
                LoadingText.text = text;
            }
            
        }
    }

    public void playButton()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void OptionButton()
    {
        UIMenu.SetActive(false);
        UIOption.SetActive(true);
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
