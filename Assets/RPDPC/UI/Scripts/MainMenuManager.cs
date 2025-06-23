using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using PDC.Localization;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class MainMenuManager : UIPanel
{
    public string GameSceneName;
    public string CreditSceneName;

    public GameObject UIOption;
    public GameObject UIMenu;
    public GameObject UILoading;
    public TextMeshProUGUI LoadingText;
    [SerializeField] Image _fade;

    [SerializeField] List<Button> _buttons;
    Action<string> OnLoadScene;

    protected override void Start()
    {
        base.Start();

        LocalizationManager.OnLocaReady(() =>
        {
            if(UILoading != null) UILoading.SetActive(false);
        });

        if(!LocalizationManager.IsLocaReady) StartCoroutine(TextAnim());
        SetInteractibleAllButton(true);
    }

    public void OnDestroy()
    {

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
        Debug.Log("Play");
        SetInteractibleAllButton(false);
        PDC.LoadSceneManager.Instance.LoadScene(GameSceneName);
        //StartCoroutine(PlayButtonCoroutine());
    }

    IEnumerator PlayButtonCoroutine()
    {
        yield return null;
        yield return _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 1f), 1.5f).WaitForCompletion();
        SetInteractibleAllButton(true);
        OnLoadScene?.Invoke(GameSceneName);
        //yield return SceneManager.LoadSceneAsync(GameSceneName);
    }

    public void OptionButton()
    {
        UIMenu.SetActive(false);
        UIOption.SetActive(true);
    }

    public void CreditButton()
    {
        Debug.Log("Credit"); 
        SetInteractibleAllButton(false);
        PDC.LoadSceneManager.Instance.LoadScene(CreditSceneName);
        //StartCoroutine(CreditButtonCoroutine());
    }

    IEnumerator CreditButtonCoroutine()
    {
        yield return null;
        if(_fade != null) yield return _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 1f), 1.5f).WaitForCompletion();
        SetInteractibleAllButton(true);
        OnLoadScene?.Invoke(CreditSceneName);
        //yield return SceneManager.LoadSceneAsync(2);
    }

    public void quitButton()
    {
        Application.Quit();
    }

    private void SetInteractibleAllButton(bool active)
    {
        foreach(var b in _buttons)
        {
            b.interactable = active;
        }
    }
}
