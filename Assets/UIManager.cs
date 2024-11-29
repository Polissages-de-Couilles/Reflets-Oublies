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

    // La fonction qui met à jour l'opacité en fonction de la santé actuelle
    public void UpdateOpacity()
    {
        float healthPercentage = player.getCurrentHealth() / player.getMaxHealth();

        if (bloodEffectImage != null)
        {
            Color currentColor = bloodEffectImage.color;
            // Diminuer l'opacité du sang à mesure que la santé augmente
            bloodEffectImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1 - healthPercentage);
        }
    }

    // Cette fonction gère les dégâts dans le UIManager
    public void InfligerDegats()
    {
        player.takeDamage(10);  // Exemple de dégâts infligés
    }

    // Fonction pour tester les soins sans changer la logique existante
    public void Heal()
    {
        player.heal(10);
        if (healthSlider != null)
        {
            healthSlider.value = player.getCurrentHealth();  // Mettre à jour le slider
        }
        UpdateOpacity();  // Mettre à jour l'opacité du sang après un soin
    }

    public void UpdateDashSlider(int dashCount)
    {
        if (dashSlider != null)
        {
            dashSlider.value = dashCount;  // Mettre à jour le slider de dash
        }
    }
}
