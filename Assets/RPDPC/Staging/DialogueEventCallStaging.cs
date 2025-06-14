using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk.Event;
using System.Linq;

[CreateAssetMenu(menuName = "Dialogue/Event/CallStaging")]
public class DialogueEventCallStaging : DialogueEventTimeEvent
{
    public string _stagingID;

    private List<StagingEvent> staging;

    public override void RunEvent()
    {
        staging = FindObjectsByType<StagingEvent>(FindObjectsSortMode.None).ToList().FindAll(x => x.ID == _stagingID/* && x.Type == _stagingType*/);
        foreach (var stag in staging)
        {
            stag.OnEventFinished += CheckAllEventFinish;
            stag.PlayEvent();
        }
    }

    private void CheckAllEventFinish()
    {
        GameManager.Instance.StartCoroutine(WaitABitBeforeChecking());
    }

    IEnumerator WaitABitBeforeChecking()
    {
        yield return null;
        Debug.Log("All Event Staging End : " + staging.TrueForAll(x => x.IsEventFinish));
        if (staging.TrueForAll(x => x.IsEventFinish))
        {
            foreach (var stag in staging)
            {
                stag.OnEventFinished -= CheckAllEventFinish;
            }
            EventEnd();
        }
    }

    protected override void EventEnd()
    {
        IsEventEnd = true;
    }
}
