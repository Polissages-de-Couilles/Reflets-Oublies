using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk;
using PDC.Localization;

public class SimpleLaunchDialogue : MonoBehaviour
{
    public DialogueContainerSO Dialogue;

    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Diagnostics.Utils.ForceCrash(0);
        LocalizationManager.OnLocaReady(() =>
        {
            DialogueManager.Instance.StartDialogue(Dialogue);
        });
    }
}
