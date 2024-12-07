using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI Elements
    [Header("UI Elements")]
    public GameObject Pause, Dialogue, HUD, MainMenu, Option;
    public GameObject GlobalOption, inputManetteOption, inputKeybordOption;

    // Scene Management
    [Header("Scene Management")]
    public string SceneGameToLoad, SceneMenuToLoad;

    // Player
    [Header("Player")]
    private PlayerDamageable player;
    public Slider healthSlider, dashSlider;

    // Effects
    [Header("Effects")]
    public GameObject bloodEffect;
    private Image bloodEffectImage;

    private void Start()
    {
        InitializePlayer();
        InitializeBloodEffect();
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.Player.Pause.performed += OnPause;
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.UI.Disable();
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.Player.Enable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log(Pause.activeSelf);
        if (!Pause.activeSelf)
        {
            Pause.SetActive(true);
        }
    }

    private void InitializePlayer()
    {
        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();
        player.OnDamageTaken += PointDeVie;

        if (healthSlider != null)
        {
            healthSlider.maxValue = player.getMaxHealth();
            healthSlider.value = player.getCurrentHealth();
        }

        PointDeVie(0, player.getCurrentHealth());
    }

    private void InitializeBloodEffect()
    {
        bloodEffectImage = bloodEffect.GetComponent<Image>();
        if (bloodEffectImage == null) return;
        UpdateOpacity();
    }

    private void PointDeVie(float degatPrit, float vieActuel)
    {
        if (healthSlider != null)
            healthSlider.value = vieActuel;

        UpdateOpacity();
    }

    public void UpdateOpacity()
    {
        if (bloodEffectImage != null)
        {
            float healthPercentage = player.getCurrentHealth() / player.getMaxHealth();
            bloodEffectImage.color = new Color(bloodEffectImage.color.r, bloodEffectImage.color.g, bloodEffectImage.color.b, 1 - healthPercentage);
        }
    }

    public void UpdateSlider()
    {
        if (healthSlider != null)
            healthSlider.value = player.getCurrentHealth();

        UpdateOpacity();
    }

    public void InfligerDegats()
    {
        player.takeDamage(10);
    }

    public void Heal()
    {
        player.heal(10);
        UpdateSlider();
    }

    public void UpdateDashSlider(int dashCount)
    {
        if (dashSlider != null)
            dashSlider.value = dashCount;
    }

    

    
}
