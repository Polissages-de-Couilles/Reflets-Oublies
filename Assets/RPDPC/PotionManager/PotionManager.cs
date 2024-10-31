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

    public TextMeshProUGUI text;

    private const float HEAL_VALUE = 0.5f;

    private PlayerInputEventManager PIE;
    private PlayerDamageable player;

    private void Start()
    {
        maxMaxPotion = 10;
        maxPotion = currentPotion;

        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Potion.performed += HealPlayer;
    }

    private void Update()
    {
        text.text = currentPotion.ToString() + "/" + maxPotion.ToString();
    }

    private void HealPlayer(InputAction.CallbackContext context)
    {
        if(currentPotion != 0 && player.currentHealth != player.maxHealth)
        {
            player.heal(player.maxHealth * HEAL_VALUE);
            currentPotion--;
        }
        else if(currentPotion == 0)
        {
            print("Saucisse pas de popo");
        }
    }

    public bool RefillPotion()
    {
        if(currentPotion <  maxPotion)
        {
            currentPotion = maxPotion;
            return true;
        }
        return false;
    }

    public bool AddMaxPotion()
    {
        if(maxPotion < maxMaxPotion)
        {
            maxPotion++;
            currentPotion++;
            return true;
        }
        return false;
    }
}
