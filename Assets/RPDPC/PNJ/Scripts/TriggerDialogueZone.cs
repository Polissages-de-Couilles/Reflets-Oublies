using MeetAndTalk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class TriggerDialogueZone : MonoBehaviour
{
    [SerializeField] DialogueContainerSO _dialogue;
    [SerializeField] int _startId = -1;
    [SerializeField] bool _isRepeatable;
    private bool isFirst = true;

    private void Awake()
    {
        isFirst = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter Dialogue" + this.name + " : " + other.gameObject);
        if(!isFirst || !other.CompareTag("Player")) return;

        GameManager.Instance.DialogueManager.OnNode += OnNode;
        GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(() =>
        {
            GameManager.Instance.DialogueManager.OnNode -= OnNode;
            EventSystem.current.SetSelectedGameObject(null);
        }
        );

        if(_startId >= 0)
        {
            GameManager.Instance.DialogueManager.SetupDialogue(_dialogue);
            GameManager.Instance.DialogueManager.StartDialogue(_startId.ToString());
        }
        else
        {
            GameManager.Instance.DialogueManager.StartDialogue(_dialogue);
        }

        if(!_isRepeatable)
        {
            isFirst = false;
        }
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
