using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI; // N�cessaire pour utiliser le Slider
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

    private PlayerDamageable player; // R�f�rence au script PlayerDamageable

    // Ajouter une r�f�rence au Slider de sant�
    public Slider healthSlider;

    // R�f�rence au GameObject dont l'opacit� va changer
    public GameObject bloodEffect;

    // R�f�rence � l'Image sur le GameObject bloodEffect
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
        // R�cup�re le PlayerDamageable attach� au joueur
        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();

        // Abonne la m�thode PointDeVie � l'�v�nement OnDamageTaken
        player.OnDamageTaken += PointDeVie;

        // Initialise le Slider de sant�
        if (healthSlider != null)
        {
            healthSlider.maxValue = player.getMaxHealth();  // D�finit la valeur maximale du slider
            healthSlider.value = player.getCurrentHealth(); // D�finit la valeur actuelle du slider
        }

        // Appeler une premi�re fois pour que le slider soit � jour au d�but
        PointDeVie(0, player.getCurrentHealth());

        // Obtenir la r�f�rence � l'Image du GameObject bloodEffect
        bloodEffectImage = bloodEffect.GetComponent<Image>();

        // V�rifie si bloodEffectImage a bien �t� assign�
        if (bloodEffectImage == null)
        {
            Debug.LogError("L'image sur bloodEffect n'est pas assign�e ou n'est pas un composant Image.");
            return;
        }

        // Initialiser l'opacit�
        UpdateOpacity();
    }

    // M�thode appel�e lorsqu'un d�g�t est pris par le joueur
    private void PointDeVie(float degatPrit, float vieActuel)
    {
        // Met � jour le slider avec la nouvelle valeur de sant�
        if (healthSlider != null)
        {
            healthSlider.value = vieActuel;
        }

        UpdateOpacity();

        // Optionnellement, tu peux ajouter d'autres comportements ici
        Debug.Log($"Sant� actuelle du joueur : {vieActuel}/{player.getMaxHealth()}");
    }

    public void UpdateOpacity()
    {
        // Calculer le pourcentage de vie restante
        float healthPercentage = player.getCurrentHealth() / player.getMaxHealth();

        // Inverser la logique de l'alpha : plus la vie est faible, plus l'opacit� est �lev�e
        // Si la vie est pleine, alpha = 0 (transparente), si la vie est � 0, alpha = 1 (opaque)
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
}
