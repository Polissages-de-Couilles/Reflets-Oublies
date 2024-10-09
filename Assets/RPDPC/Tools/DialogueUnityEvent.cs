using MeetAndTalk.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialogue/Event/UnityEvent")]
[System.Serializable]
public class DialogueUnityEvent : DialogueEventSO
{
    public UnityEvent _event;
    public override void RunEvent()
    {
        _event?.Invoke();
    }
}
