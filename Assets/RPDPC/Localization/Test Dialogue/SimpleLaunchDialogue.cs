using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeetAndTalk;
using PDC.Localization;
using UnityEngine.EventSystems;

public class SimpleLaunchDialogue : MonoBehaviour
{
    public DialogueContainerSO Dialogue;

    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Diagnostics.Utils.ForceCrash(0);
        LocalizationManager.OnLocaReady(() =>
        {
            GameManager.Instance.DialogueManager.OnNode += OnNode;
            GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(() =>
            {
                GameManager.Instance.DialogueManager.OnNode -= OnNode;
                EventSystem.current.SetSelectedGameObject(null);
            }
            );
            //_dialogues.Find(x => x.Act == GameManager.Instance.StoryManager.CurrentAct);
            GameManager.Instance.DialogueManager.StartDialogue(Dialogue);
        });
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
