using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI; // Nécessaire pour utiliser le Slider
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject Pause;
    public GameObject Dialogue;
    public GameObject HUD;
    public GameObject MainMenu;
    public GameObject Option;

    public string SceneGameToLoad;
    public string SceneMenuToLoad;

    public GameObject GlobalOption;
    public GameObject inputManetteOption;
    public GameObject inputKeybordOption;

    private PlayerDamageable player; 
    
    public Slider healthSlider;
    public Slider dashSlider;

    
    public GameObject bloodEffect;
    private Image bloodEffectImage;

    public void SwitchOption()
    {
        Option.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void SwitchMainMenu()
    {
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

    public void ShowGlobalOption()
    {
        GlobalOption.SetActive(true);
        inputKeybordOption.SetActive(false);
        inputManetteOption.SetActive(false);
    }

    public void ShowInputOption()
    {
        GlobalOption.SetActive(false);
        if (Gamepad.all.Count > 0)
        {
            inputManetteOption.SetActive(true);
        }
        else
        {
            inputKeybordOption.SetActive(true);
        }
    }

    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();

        player.OnDamageTaken += PointDeVie;

        if (healthSlider != null)
        {
            healthSlider.maxValue = player.getMaxHealth();  
            healthSlider.value = player.getCurrentHealth(); 
        }

        PointDeVie(0, player.getCurrentHealth());

        bloodEffectImage = bloodEffect.GetComponent<Image>();

        if (bloodEffectImage == null)
        {
            Debug.LogError("L'image sur bloodEffect n'est pas assignée ou n'est pas un composant Image.");
            return;
        }

        UpdateOpacity();
    }

    private void PointDeVie(float degatPrit, float vieActuel)
    {
        if (healthSlider != null)
        {
            healthSlider.value = vieActuel;
        }

        UpdateOpacity();

        Debug.Log($"Santé actuelle du joueur : {vieActuel}/{player.getMaxHealth()}");
    }

    public void UpdateOpacity()
    {
        float healthPercentage = player.getCurrentHealth() / player.getMaxHealth();

        if (bloodEffectImage != null)
        {
            Color currentColor = bloodEffectImage.color;
            bloodEffectImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1 - healthPercentage);
        }
    }

    public void InfligerDegats()
    {
        player.takeDamage(10);
    }

    public void UpdateDashSlider(int dashCount)
    {
        if (dashSlider != null)
        {
            dashSlider.value = dashCount;  // Met à jour la valeur du slider avec le nombre de dashes restants
        }
    }

}
