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

    private PlayerDamageable player; // Référence au script PlayerDamageable

    // Ajouter une référence au Slider de santé
    public Slider healthSlider;

    // Référence au GameObject dont l'opacité va changer
    public GameObject bloodEffect;

    // Référence à l'Image sur le GameObject bloodEffect
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
        // Récupère le PlayerDamageable attaché au joueur
        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();

        // Abonne la méthode PointDeVie à l'événement OnDamageTaken
        player.OnDamageTaken += PointDeVie;

        // Initialise le Slider de santé
        if (healthSlider != null)
        {
            healthSlider.maxValue = player.getMaxHealth();  // Définit la valeur maximale du slider
            healthSlider.value = player.getCurrentHealth(); // Définit la valeur actuelle du slider
        }

        // Appeler une première fois pour que le slider soit à jour au début
        PointDeVie(0, player.getCurrentHealth());

        // Obtenir la référence à l'Image du GameObject bloodEffect
        bloodEffectImage = bloodEffect.GetComponent<Image>();

        // Vérifie si bloodEffectImage a bien été assigné
        if (bloodEffectImage == null)
        {
            Debug.LogError("L'image sur bloodEffect n'est pas assignée ou n'est pas un composant Image.");
            return;
        }

        // Initialiser l'opacité
        UpdateOpacity();
    }

    // Méthode appelée lorsqu'un dégât est pris par le joueur
    private void PointDeVie(float degatPrit, float vieActuel)
    {
        // Met à jour le slider avec la nouvelle valeur de santé
        if (healthSlider != null)
        {
            healthSlider.value = vieActuel;
        }

        UpdateOpacity();

        // Optionnellement, tu peux ajouter d'autres comportements ici
        Debug.Log($"Santé actuelle du joueur : {vieActuel}/{player.getMaxHealth()}");
    }

    public void UpdateOpacity()
    {
        // Calculer le pourcentage de vie restante
        float healthPercentage = player.getCurrentHealth() / player.getMaxHealth();

        // Inverser la logique de l'alpha : plus la vie est faible, plus l'opacité est élevée
        // Si la vie est pleine, alpha = 0 (transparente), si la vie est à 0, alpha = 1 (opaque)
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
