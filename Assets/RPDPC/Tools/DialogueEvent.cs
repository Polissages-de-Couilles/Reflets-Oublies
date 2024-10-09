using MeetAndTalk.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Event/Event")]
[System.Serializable]
public class DialogueEvent : DialogueEventSO
{
    public Action _event;
    public override void RunEvent()
    {
        _event?.Invoke();
    }
}
