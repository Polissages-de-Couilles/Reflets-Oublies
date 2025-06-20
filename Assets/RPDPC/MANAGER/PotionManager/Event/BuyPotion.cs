using MeetAndTalk.Event;
using MeetAndTalk.GlobalValue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Event/Buy_Potion")]
[System.Serializable]
public class BuyPotion : DialogueEventSO
{
    public int potionValue;
    public override void RunEvent()
    {
        if (GameManager.Instance.PotionManager.AddMaxPotion(false) && GameManager.Instance.MoneyManager.PlayerMonney >= potionValue)
        {
            GameManager.Instance.PotionManager.AddMaxPotion(true);
            GameManager.Instance.MoneyManager.ChangePlayerMonney(-potionValue);

            bool canBuy = Mathf.Clamp(GameManager.Instance.MemoryManager.EncounteredMemory.FindAll(x => x._isTaken).Count + 1, 0, 5) > GameManager.Instance.PotionManager.MaxPotion;
            var manager = Resources.Load<GlobalValueManager>("GlobalValue");
            manager.LoadFile();
            manager.Set("CAN_BUY_POTION_NB", (canBuy ? 1 : 0).ToString());
        }

    }
}
