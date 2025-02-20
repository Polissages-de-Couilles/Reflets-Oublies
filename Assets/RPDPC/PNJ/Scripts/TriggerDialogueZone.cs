using MeetAndTalk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueZone : MonoBehaviour
{
    [SerializeField] DialogueContainerSO _dialogue;

    public void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.DialogueManager.StartDialogue(_dialogue);
    }
}
