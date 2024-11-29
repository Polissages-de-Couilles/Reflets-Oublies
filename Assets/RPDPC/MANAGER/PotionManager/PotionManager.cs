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

    private void Start()
    {
        maxMaxPotion = 10;
        maxPotion = currentPotion;

        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Potion.performed += HealPlayer;
    }

    private void HealPlayer(InputAction.CallbackContext context)
    {
        if(currentPotion != 0 && player.getCurrentHealth() != player.maxHealth)
        {
            player.heal(player.maxHealth * HEAL_VALUE);
            currentPotion--;
        }
        else if(currentPotion == 0)
        {
            print("Saucisse pas de popo");
        }
    }

    public bool RefillPotion(bool check)
    {
        if(currentPotion <  maxPotion)
        {
            if (check == false) { currentPotion = maxPotion; }
            return true;
        }
        return false;
    }

    public bool AddMaxPotion(bool check)
    {
        if(maxPotion < maxMaxPotion)
        {
            if(check == false)
            {
                maxPotion++;
                currentPotion++;
            }
            return true;
        }
        return false;
    }
}
