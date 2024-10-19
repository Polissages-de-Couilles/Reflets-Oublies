using MeetAndTalk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC.Localization;

public class PNJ : Interactible
{
    [SerializeField] DialogueContainerSO dialogue;

    public override void OnInteraction()
    {
        if (!LocalizationManager.IsLocaReady) return;
        GameManager.Instance.DialogueManager.StartDialogue(dialogue);
    }
}
