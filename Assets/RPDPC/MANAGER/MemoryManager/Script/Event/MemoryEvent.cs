using MeetAndTalk.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Event/Memory_Event")]
[System.Serializable]
public class MemoryEvent : DialogueEventSO
{
    public MemorySO memory;
    public override void RunEvent()
    {
        memory.RunEvent(true);
    }
}
