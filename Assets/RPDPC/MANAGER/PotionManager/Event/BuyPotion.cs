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

            var manager = Resources.Load<GlobalValueManager>("GlobalValue");
            manager.LoadFile();
            var value = manager.BoolValues.Find(x => x.ValueName.Equals("CAN_BUY_POTION"));
            bool canBuy = Mathf.Clamp(GameManager.Instance.MemoryManager.EncounteredMemory.FindAll(x => x._isTaken).Count + 1, 0, 5) > GameManager.Instance.PotionManager.MaxPotion;
            value.Value = canBuy;
        }

    }
}
