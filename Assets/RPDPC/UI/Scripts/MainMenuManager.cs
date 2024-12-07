using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : UIPanel
{
    public string GameSceneName;

    public GameObject UIOption;
    public GameObject UIMenu;

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
