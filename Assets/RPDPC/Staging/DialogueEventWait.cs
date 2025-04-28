using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk.Event;
using System.Linq;

[CreateAssetMenu(menuName = "Dialogue/Event/Wait")]
public class DialogueEventWait : DialogueEventTimeEvent
{
    public float Duration;

    public override void RunEvent()
    {
        IEnumerator tmp() { yield return new WaitForSeconds(Duration); EventEnd(); }
        GameManager.Instance.StartCoroutine(tmp());
    }

    protected override void EventEnd()
    {
        base.EventEnd();
    }
}
