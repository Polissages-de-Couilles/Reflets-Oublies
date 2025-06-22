using DG.Tweening;
using MeetAndTalk.GlobalValue;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
    private int maxMaxPotion;
    public int MaxPotion => maxPotion;
    private int maxPotion = 1;
    public int currentPotion = 1;

    private const float HEAL_VALUE = 0.5f;

    private PlayerInputEventManager PIE;
    private PlayerDamageable player;

    [Header("Ui")]
    private UIManager uiManager;  
    public TextMeshProUGUI text;
    public GameObject Potion;
    private Vector3 lastPos;
    [SerializeField] List<DoubleImage> potionSprite;
    Image imagePotion;
    public GlobalValueInt CanBuyPotion { get; set; }

    private void Awake()
    {
        currentPotion = 1;
    }

    private void Start()
    {
        lastPos = Potion.transform.position;

        maxMaxPotion = 5;
        maxPotion = currentPotion;
        imagePotion = Potion.GetComponent<Image>();
        SetPotionImage(maxPotion, currentPotion > 0);

        text.text = currentPotion.ToString();

        player = GameManager.Instance.Player.GetComponent<PlayerDamageable>();
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Potion.performed += HealPlayer;

        uiManager = GameManager.Instance.UIManager;

        var manager = Resources.Load<GlobalValueManager>("GlobalValue");
        manager.LoadFile();
        //CanBuyPotion = manager.IntValues.Find(x => x.ValueName.Equals("CAN_BUY_POTION_NB"));
        bool canBuy = Mathf.Clamp(GameManager.Instance.MemoryManager.EncounteredMemory.FindAll(x => x._isTaken).Count + 1, 0, 5) > GameManager.Instance.PotionManager.MaxPotion;
        manager.Set("CAN_BUY_POTION_NB", (canBuy ? 1 : 0).ToString());
    }

    private void HealPlayer(InputAction.CallbackContext context)
    {
        if (currentPotion != 0 && player.getCurrentHealth() != player.maxHealth)
        {
            player.heal(player.maxHealth * HEAL_VALUE);
            currentPotion--;
            text.text = currentPotion.ToString();
            SetPotionImage(maxPotion, currentPotion > 0);

            if (uiManager != null)
            {
                uiManager.UpdateSlider();
            }

            //var go = Instantiate(vfx, GameManager.Instance.Player.transform);
            //sfxSource.PlayOneShot(sfx);
            //go.transform.localPosition = new(0, -0.5f, 0);
            //Debug.Log("Heal : " + go.transform.localPosition);
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
            if (refillPotion == true)
            {
                currentPotion = maxPotion;
                text.text = currentPotion.ToString();
                SetPotionImage(maxPotion, currentPotion > 0);
            }
            return true;
        }
        return false;
    }

    public bool AddMaxPotion(bool addMaxPotion)
    {
        if (maxPotion < maxMaxPotion)
        {
            if(addMaxPotion == true)
            {
                maxPotion++;
                currentPotion++;
                text.text = currentPotion.ToString();
                SetPotionImage(maxPotion, currentPotion > 0);
            }
            return true;
        }
        return false;
    }

    public void doShake()
    {
        Potion.transform.DOShakePosition(1, 2);
    }

    private void SetPotionImage(int maxPotion, bool isFill = true)
    {
        //Debug.Log("Set Potion Image : " + maxPotion);
        imagePotion.sprite = isFill ? potionSprite[maxPotion - 1].Fill : potionSprite[maxPotion - 1].Empty;
        (imagePotion.transform as RectTransform).sizeDelta = potionSprite[maxPotion - 1].Size;
    }

    [System.Serializable]
    public struct DoubleImage
    {
        public Sprite Fill;
        public Sprite Empty;
        public Vector2 Size;
    }
}
