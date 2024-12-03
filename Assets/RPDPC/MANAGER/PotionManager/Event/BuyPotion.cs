using MeetAndTalk.Event;
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
        //if (GameManager.Instance.PotionManager.AddMaxPotion()) GameManager.Instance.MoneyManager.ChangePlayerMonney(-potionValue);
    }
}
