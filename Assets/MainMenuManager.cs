using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public string GameSceneName;

    public GameObject UIOption;
    public GameObject UIMenu;

    public EventSystem UINavigation;
    public GameObject btnFocusOption;

    void Start()
    {
        UINavigation = EventSystem.current;
    }



    public void playButton()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void OptionButton()
    {
        UIOption.SetActive(true);
        UIMenu.SetActive(false);
        UINavigation.SetSelectedGameObject(btnFocusOption);
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
