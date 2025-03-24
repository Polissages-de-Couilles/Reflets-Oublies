using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk.Event;
using System.Linq;

public abstract class DialogueEventTimeEvent : DialogueEventSO
{
    public bool IsEventEnd { get; set; } = false;

    protected virtual void EventEnd()
    {
        IsEventEnd = true;
    }
}
