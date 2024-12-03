using MeetAndTalk.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Event/RefillPotion")]
[System.Serializable]
public class RefillPotion : DialogueEventSO
{
    public int potionValue;
    public override void RunEvent()
    {
        //if (GameManager.Instance.PotionManager.RefillPotion()) 
         //   GameManager.Instance.MoneyManager.ChangePlayerMonney(-potionValue);
    }
}
