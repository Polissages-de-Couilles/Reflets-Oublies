using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : UIPanel
{
    public string MainMenuScene;

    public GameObject UIOption;
    public GameObject UIMenu;

    public void playButton()
    {
        //Time.timeScale = 1;
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.UI.Disable();
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.Player.Enable();
        UIOption.SetActive(false);
        UIMenu.SetActive(false);
    }

    public void OptionButton()
    {
        UIMenu.SetActive(false);
        UIOption.SetActive(true);
    }

    public void quitButton()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.UI.Enable();
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.Player.Disable();
        //Time.timeScale = 0;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
