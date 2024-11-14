using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject Pause;
    public GameObject Dialogue;
    public GameObject HUD;
    public GameObject MainMenu;
    public GameObject Option;

    public String SceneGameToLoad;
    public String SceneMenuToLoad;

    public void SwitchOption() {
        Option.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void SwitchMainMenu() {
        Option.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void SwitchSceneMenu()
    {
        SceneManager.LoadScene(SceneMenuToLoad);
    }

    public void SwitchSceneGame()
    {
        SceneManager.LoadScene(SceneGameToLoad);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitter !");
    }

    public void ShowPause()
    {
        Pause.SetActive(true);
    }

    public void MaskPause()
    {
        Pause.SetActive(false);
    }



}
