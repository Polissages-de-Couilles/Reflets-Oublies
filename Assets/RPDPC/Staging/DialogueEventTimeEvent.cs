using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk.Event;
using System.Linq;

public class DialogueEventTimeEvent : DialogueEventSO
{
    public bool IsEventEnd { get; set; } = false;

    public override void RunEvent()
    {

    }

    protected virtual void EventEnd()
    {
        IsEventEnd = true;
    }
}
