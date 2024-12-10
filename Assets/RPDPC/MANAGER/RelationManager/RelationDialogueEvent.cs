using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeetAndTalk.Event
{
    [CreateAssetMenu(menuName = "Dialogue/Event/Relation")]
    public class RelationDialogueEvent : DialogueEventSO
    {
        public float relationValue;

        public override void RunEvent()
        {
            GameManager.Instance.RelationManager.ChangeValue(relationValue);
        }
    }
}