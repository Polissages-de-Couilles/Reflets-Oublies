using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk.Event;
using System.Linq;

[CreateAssetMenu(menuName = "Dialogue/Event/CallStaging")]
public class DialogueEventCallStaging : DialogueEventTimeEvent
{
    public string _stagingID;
    public StagingEvent.StagingEventTypes _stagingType;

    private StagingEvent staging;

    public override void RunEvent()
    {
        staging = FindObjectsByType<StagingEvent>(FindObjectsSortMode.None).ToList().Find(x => x.ID == _stagingID && x.Type == _stagingType);
        staging.OnEventFinished += EventEnd;
        staging.PlayEvent();
    }

    protected override void EventEnd()
    {
        IsEventEnd = true;
        staging.OnEventFinished -= EventEnd;
    }
}
