using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PotionManager : MonoBehaviour
{
    private int maxMaxPotion;
    private int maxPotion;
    public int currentPotion;

    private const float HEAL_VALUE = 0.5f;

    private PlayerInputEventManager PIE;
    private PlayerDamageable player;

    public TextMeshProUGUI text;

    private UIManager uiManager;  // Référence au UIManager

    private void Start()
    {
        maxMaxPotion = 10;
        maxPotion = currentPotion;

        text.text = currentPotion.ToString();

        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Potion.performed += HealPlayer;

        uiManager = GameManager.Instance.UIManager;
    }

    private void HealPlayer(InputAction.CallbackContext context)
    {
        if (currentPotion != 0 && player.getCurrentHealth() != player.maxHealth)
        {
            player.heal(player.maxHealth * HEAL_VALUE);
            currentPotion--;
            text.text = currentPotion.ToString();

            if (uiManager != null)
            {
                uiManager.UpdateSlider();
            }
        }
        else if (currentPotion == 0)
        {
            print("Saucisse pas de popo");
        }
    }

    // Temporaire, à retirer
    public void TestHealPlayer()
    {
        if (currentPotion != 0 && player.getCurrentHealth() != player.maxHealth)
        {
            player.heal(player.maxHealth * HEAL_VALUE);
            currentPotion--;
            text.text = currentPotion.ToString();

            if (uiManager != null)
            {
                uiManager.UpdateSlider();
            }
        }
        else if (currentPotion == 0)
        {
            print("Saucisse pas de popo");
        }
    }

    public void RefillPotion()
    {
        if (currentPotion < maxPotion)
        {
            currentPotion = maxPotion;
            text.text = currentPotion.ToString();
            return;
        }
        return;
    }

    public void AddMaxPotion()
    {
        if (maxPotion < maxMaxPotion)
        {
            maxPotion++;
            currentPotion++;
            text.text = currentPotion.ToString();
            return;
        }
        return;
    }
}
