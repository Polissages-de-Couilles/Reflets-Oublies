using MeetAndTalk.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Event/Memory_Event")]
[System.Serializable]
public class MemoryEvent : DialogueEventSO
{
    public MemorySO memory;
    public bool take;

    public override void RunEvent()
    {
        memory.RunEvent(take);
    }
}
