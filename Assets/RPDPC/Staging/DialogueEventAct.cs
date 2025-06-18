using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventAct : DialogueEventTimeEvent
{
    public Act toAct;
    public override void RunEvent()
    {
        GameManager.Instance.StoryManager.SwitchAct(toAct);

        EventEnd();
    }
}
