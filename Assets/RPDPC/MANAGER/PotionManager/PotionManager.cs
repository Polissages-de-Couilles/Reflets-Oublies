using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PotionManager : MonoBehaviour
{
    private int maxMaxPotion;
    private int maxPotion;
    public int currentPotion;

    private const float HEAL_VALUE = 0.5f;

    private PlayerInputEventManager PIE;
    private PlayerDamageable player;

    [Header("Ui")]
    private UIManager uiManager;  
    public TextMeshProUGUI text;
    public GameObject Potion;
    private Vector3 lastPos;

    private void Start()
    {
        lastPos = Potion.transform.position;

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
            Potion.transform.DOShakePosition(1, 2).OnComplete(() => Potion.transform.position = lastPos);
        }
    }

    public bool RefillPotion(bool refillPotion)
    {
        if (currentPotion < maxPotion)
        {
            currentPotion = maxPotion;
            text.text = currentPotion.ToString();
            return true;
        }
        return false;
    }

    public bool AddMaxPotion(bool addMaxPotion)
    {
        if (maxPotion < maxMaxPotion)
        {
            maxPotion++;
            currentPotion++;
            text.text = currentPotion.ToString();
            return true;
        }
        return false;
    }

    public void doShake()
    {
        Potion.transform.DOShakePosition(1, 2);
    }
}
