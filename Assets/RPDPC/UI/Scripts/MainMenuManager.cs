using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using PDC.Localization;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuManager : UIPanel
{
    public string GameSceneName;
    public string CreditSceneName;

    public GameObject UIOption;
    public GameObject UIMenu;
    public GameObject UILoading;
    public TextMeshProUGUI LoadingText;
    [SerializeField] Image _fade;

    bool isLoadScene = false;

    protected override void Start()
    {
        base.Start();
        LocalizationManager.OnLocaReady(() =>
        {
            if(UILoading != null) UILoading.SetActive(false);
        });

        StartCoroutine(TextAnim());
        isLoadScene = false;
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
        if (isLoadScene) return;
        isLoadScene = true;
        StartCoroutine(PlayButtonCoroutine());
    }

    IEnumerator PlayButtonCoroutine()
    {
        yield return _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 1f), 1.5f).WaitForCompletion();
        isLoadScene = false;
        SceneManager.LoadScene(GameSceneName);
    }

    public void OptionButton()
    {
        if (isLoadScene) return;
        UIMenu.SetActive(false);
        UIOption.SetActive(true);
    }

    public void CreditButton()
    {
        if(isLoadScene) return;
        isLoadScene = true;
        StartCoroutine(CreditButtonCoroutine());
    }

    IEnumerator CreditButtonCoroutine()
    {
        yield return _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 1f), 1.5f).WaitForCompletion();
        isLoadScene = false;
        SceneManager.LoadScene(CreditSceneName);
    }

    public void quitButton()
    {
        if (isLoadScene) return;
        Application.Quit();
    }
}
