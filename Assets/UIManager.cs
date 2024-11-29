using System;
using System.Collections;
using System.Collections.Generic;
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
            Debug.LogError("L'image sur bloodEffect n'est pas assign�e ou n'est pas un composant Image.");
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

        Debug.Log($"Sant� actuelle du joueur : {vieActuel}/{player.getMaxHealth()}");
    }

    // La fonction qui met � jour l'opacit� en fonction de la sant� actuelle
    public void UpdateOpacity()
    {
        float healthPercentage = player.getCurrentHealth() / player.getMaxHealth();

        if (bloodEffectImage != null)
        {
            Color currentColor = bloodEffectImage.color;
            // Diminuer l'opacit� du sang � mesure que la sant� augmente
            bloodEffectImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1 - healthPercentage);
        }
    }

    // Cette fonction g�re les d�g�ts dans le UIManager
    public void InfligerDegats()
    {
        player.takeDamage(10);  // Exemple de d�g�ts inflig�s
    }

    // Fonction pour tester les soins sans changer la logique existante
    public void Heal()
    {
        player.heal(10);
        if (healthSlider != null)
        {
            healthSlider.value = player.getCurrentHealth();  // Mettre � jour le slider
        }
        UpdateOpacity();  // Mettre � jour l'opacit� du sang apr�s un soin
    }

    public void UpdateDashSlider(int dashCount)
    {
        if (dashSlider != null)
        {
            dashSlider.value = dashCount;  // Mettre � jour le slider de dash
        }
    }
}
