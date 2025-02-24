using MeetAndTalk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class TriggerDialogueZone : MonoBehaviour
{
    [SerializeField] DialogueContainerSO _dialogue;

    public void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.DialogueManager.OnNode += OnNode;
        GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(() =>
        {
            GameManager.Instance.DialogueManager.OnNode -= OnNode;
            EventSystem.current.SetSelectedGameObject(null);
        }
        );
        GameManager.Instance.DialogueManager.StartDialogue(_dialogue);
    }

    private void OnNode(BaseNodeData data)
    {
        if(data is DialogueNodeData)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameManager.Instance.DialogueUIManager.SkipButton);
            return;
        }

        if(data is DialogueChoiceNodeData || data is TimerChoiceNodeData)
        {
            EventSystem.current.SetSelectedGameObject(null);
            GameManager.Instance.DialogueUIManager.OnButtonCreate += SelectChoiceButton;
            return;
        }
    }

    private void SelectChoiceButton(int textcount)
    {
        if(textcount == 0) return;
        GameManager.Instance.DialogueUIManager.OnButtonCreate -= SelectChoiceButton;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameManager.Instance.DialogueUIManager.ButtonContainer.transform.GetChild(0).gameObject);
    }
}
