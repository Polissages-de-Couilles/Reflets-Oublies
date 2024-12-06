using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
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
        UIOption.SetActive(true);
        UIMenu.SetActive(false);
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
